using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ServerApp
{
    public class SocketServer
    {
        private TcpListener _listener;
        private Thread _listenThread;
        private bool _isRunning;
        private int _port = 9999;

        // Danh sách quản lý các Client đang kết nối thật (Đã chuyển sang ClientInfo)
        public List<ClientInfo> ConnectedClients { get; set; } = new List<ClientInfo>();

        // Sự kiện thông báo cho Giao diện (UI) biết khi có Client vừa Online hoặc Offline
        public event Action OnClientListChanged;
        public event Action<string> OnChatReceived;
        public event Action<string> OnFilesReceived;

        // Singleton Pattern để mọi Form trong ServerApp đều dùng chung 1 kết nối Socket ngầm
        private static SocketServer _instance;
        public static SocketServer Instance => _instance ?? (_instance = new SocketServer());

        private SocketServer() { }

        // Hàm khởi động Server lắng nghe kết nối
        public void Start(int port)
        {
            if (_isRunning) return;
            this._port = port;

            try
            {
                _listener = new TcpListener(IPAddress.Any, this._port);
                _listener.Start();
                _isRunning = true;

                // Tạo một luồng chạy độc lập để tránh làm treo đơ giao diện Form chính
                _listenThread = new Thread(ListenForClients);
                _listenThread.IsBackground = true;
                _listenThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể mở cổng kết nối: {ex.Message}", "Lỗi Socket", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Stop()
        {
            if (!_isRunning) return;

            try
            {
                _isRunning = false;

                // Đóng Listener để nhả cổng mạng ra ngay lập tức
                if (_listener != null)
                {
                    _listener.Stop();
                    _listener = null;
                }

                // Đóng toàn bộ kết nối của các Client đang Online
                lock (ConnectedClients)
                {
                    foreach (var client in ConnectedClients)
                    {
                        if (client.Socket != null)
                        {
                            client.Socket.Close(); // ĐÃ SỬA DÒNG 75: Chọc vào .Socket để Close()
                        }
                    }
                    ConnectedClients.Clear();
                }

                // Kích hoạt sự kiện để UI biết danh sách đã về 0
                OnClientListChanged?.Invoke();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi dừng Server: {ex.Message}");
            }
        }

        // Vòng lặp liên tục chờ máy con (Client) kết nối vào
        private void ListenForClients()
        {
            while (_isRunning)
            {
                try
                {
                    TcpClient client = _listener.AcceptTcpClient();

                    // Tạo luồng độc lập xử lý quá trình bắt tay và đọc dữ liệu cho từng Client
                    // Việc này giúp Server không bị nghẽn ở luồng chính khi xử lý nhiều máy cùng lúc
                    Thread clientThread = new Thread(() =>
                    {
                        try
                        {
                            NetworkStream stream = client.GetStream();

                            // Cấu hình thời gian chờ đọc gói tin đầu tiên là 5 giây
                            // Tránh việc Client kết nối ảo làm treo luồng của Server
                            client.ReceiveTimeout = 5000;

                            byte[] buffer = new byte[1024];
                            int bytesRead = stream.Read(buffer, 0, buffer.Length);
                            string firstMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                            MessageBox.Show(firstMessage);
                            if (bytesRead == 0) return;

                            //string firstMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                            string clientName = "Ẩn danh";

                            if (firstMessage.StartsWith("CONNECT|"))
                            {
                                clientName = firstMessage.Split('|')[1];
                            }

                            // Khôi phục lại trạng thái chờ vô hạn cho các gói tin Chat/File sau đó
                            client.ReceiveTimeout = 0;

                            // Tạo đối tượng ClientInfo và nạp vào danh sách quản lý
                            ClientInfo clientInfo = new ClientInfo(client, clientName);
                            lock (ConnectedClients)
                            {
                                ConnectedClients.Add(clientInfo);
                            }

                            // Cập nhật danh sách hiển thị lên giao diện UI
                            OnClientListChanged?.Invoke();

                            // Chuyển tiếp sang hàm xử lý nhận tin nhắn và nhận file trường kỳ
                            HandleClientComm(clientInfo);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Lỗi thiết lập ban đầu của Client: {ex.Message}");
                            client.Close();
                        }
                    });

                    clientThread.IsBackground = true;
                    clientThread.Start();
                }
                catch
                {
                    break;
                }
            }
        }

        // Hàm xử lý tương tác riêng biệt với từng máy Client
        private void HandleClientComm(ClientInfo clientInfo)
        {
            TcpClient client = clientInfo.Socket;
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[8192];
            int bytesRead;

            IPEndPoint ipEnd = (IPEndPoint)client.Client.RemoteEndPoint;

            while (_isRunning && client.Connected)
            {
                try
                {
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                }
                catch
                {
                    break;
                }

                if (bytesRead == 0) break;

                string rawData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                MessageBox.Show(rawData, "Server nhận được");

                // 1. XỬ LÝ NHẬN TIN CHAT TỪ CLIENT
                if (rawData.StartsWith("CHAT|"))
                {
                    string chatMessage = rawData.Substring(5);
                    string displayName = (clientInfo != null) ? clientInfo.Name : "Client";
                    string displayIP = (ipEnd != null) ? ipEnd.Address.ToString() : "Unknown";
                    string time = DateTime.Now.ToString("HH:mm:ss");
                    string fullMessage =
                        $"[{time}] [{displayName} - {displayIP}]:{Environment.NewLine}{chatMessage}";
                    MessageBox.Show(fullMessage);
                    // Hiển thị trên Server
                    OnChatReceived?.Invoke(fullMessage);

                    // Gửi cho tất cả Client
                    BroadcastMessage(fullMessage);
                }
                // 2. XỬ LÝ NHẬN FILE TỪ CLIENT
                else if (rawData.StartsWith("FILE_STREAM|"))
                {
                    try
                    {
                        string[] fileParts = rawData.Split('|');
                        string fileName = fileParts[1];
                        int fileSize = int.Parse(fileParts[2]);

                        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                        string savePath = Path.Combine(desktopPath, fileName);

                        string fullHeader = $"FILE_STREAM|{fileName}|{fileSize}|";
                        int headerLength = Encoding.UTF8.GetByteCount(fullHeader);

                        using (FileStream fs = new FileStream(savePath, FileMode.Create, FileAccess.Write))
                        {
                            int firstChunkSize = bytesRead - headerLength;
                            if (firstChunkSize > 0)
                            {
                                fs.Write(buffer, headerLength, firstChunkSize);
                            }

                            int totalBytesReceived = firstChunkSize;
                            while (totalBytesReceived < fileSize)
                            {
                                int currentRead = stream.Read(buffer, 0, Math.Min(buffer.Length, fileSize - totalBytesReceived));
                                if (currentRead == 0) break;

                                fs.Write(buffer, 0, currentRead);
                                totalBytesReceived += currentRead;
                            }
                        }

                        OnFilesReceived?.Invoke($"[Hệ thống]: Đã nhận file '{fileName}' ({fileSize} bytes) từ Client {clientInfo.Name} lưu tại Desktop.");
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Lỗi bóc tách file: {ex.Message}");
                    }
                }
            }

            lock (ConnectedClients)
            {
                ConnectedClients.Remove(clientInfo);
            }
            OnClientListChanged?.Invoke();
        }

        private void BroadcastMessage(string fullMessage)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(fullMessage);
            byte[] lengthBytes = BitConverter.GetBytes(messageBytes.Length);

            lock (ConnectedClients)
            {
                foreach (var client in ConnectedClients)
                {
                    try
                    {
                        if (client.Socket != null && client.Socket.Connected)
                        {
                            NetworkStream stream = client.Socket.GetStream();

                            // Header chat
                            stream.WriteByte(0x01);

                            // Độ dài
                            stream.Write(lengthBytes, 0, 4);

                            // Nội dung
                            stream.Write(messageBytes, 0, messageBytes.Length);

                            stream.Flush();
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
}