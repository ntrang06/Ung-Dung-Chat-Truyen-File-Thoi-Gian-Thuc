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

                    NetworkStream stream = client.GetStream();
                    byte[] buffer = new byte[1024];

                    // ĐÃ SỬA: Thêm vòng lặp kiểm tra DataAvailable để đợi gói tin vật lý truyền từ card mạng về hoàn tất
                    while (!stream.DataAvailable && _isRunning)
                    {
                        Thread.Sleep(10); // Nghỉ 10ms để nhường CPU, đợi dữ liệu từ mạng đổ về bộ đệm
                    }

                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string firstMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    string clientName = "Ẩn danh";
                    if (firstMessage.StartsWith("CONNECT|"))
                    {
                        clientName = firstMessage.Split('|')[1];
                    }

                    // Tạo thực thể ClientInfo mới
                    ClientInfo clientInfo = new ClientInfo(client, clientName);

                    lock (ConnectedClients)
                    {
                        ConnectedClients.Add(clientInfo);
                    }

                    // Kích hoạt sự kiện cập nhật danh sách lên giao diện
                    OnClientListChanged?.Invoke();

                    // Tạo luồng đọc dữ liệu riêng cho từng Client
                    Thread clientThread = new Thread(() => HandleClientComm(clientInfo));
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
            TcpClient client = clientInfo.Socket; // Lấy Socket từ ClientInfo ra phục vụ logic đọc mạng cũ
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[8192]; // Tăng buffer lên 8KB để nhận file mượt hơn
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

                // Kiểm tra xem là gói tin TEXT hay FILE_STREAM bằng mảng byte thô đầu tiên
                string headerCheck = Encoding.UTF8.GetString(buffer, 0, Math.Min(bytesRead, 20));

                // 1. Xử lý gói tin CHAT thông thường (Có hiển thị kèm theo Tên người dùng)
                if (headerCheck.StartsWith("CHAT|"))
                {
                    string rawData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    string chatMessage = rawData.Substring(5);
                    OnChatReceived?.Invoke($"[{clientInfo.Name} - {ipEnd.Address}]: {chatMessage}");
                }
                // 2. Xử lý nhận gói tin FILE_STREAM (Tách header văn bản và mảng byte thô)
                else if (headerCheck.StartsWith("FILE_STREAM|"))
                {
                    try
                    {
                        // Đọc toàn bộ chuỗi thô để định vị các dấu phân cách '|'
                        string rawHeaderString = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        string[] fileParts = rawHeaderString.Split('|');

                        string fileName = fileParts[1];
                        int fileSize = int.Parse(fileParts[2]);

                        // Tạo đường dẫn lưu file trực tiếp ra Desktop
                        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                        string savePath = Path.Combine(desktopPath, fileName);

                        // Tìm vị trí kết thúc của Header để lấy vị trí bắt đầu của dữ liệu nhị phân
                        string fullHeader = $"FILE_STREAM|{fileName}|{fileSize}|";
                        int headerLength = Encoding.UTF8.GetByteCount(fullHeader);

                        using (FileStream fs = new FileStream(savePath, FileMode.Create, FileAccess.Write))
                        {
                            // Ghi phần mảng byte của file còn dư nằm trong buffer ban đầu
                            int firstChunkSize = bytesRead - headerLength;
                            if (firstChunkSize > 0)
                            {
                                fs.Write(buffer, headerLength, firstChunkSize);
                            }

                            // Tiếp tục vòng lặp đọc hết số byte còn lại của file từ NetworkStream
                            int totalBytesReceived = firstChunkSize;
                            while (totalBytesReceived < fileSize)
                            {
                                int currentRead = stream.Read(buffer, 0, Math.Min(buffer.Length, fileSize - totalBytesReceived));
                                if (currentRead == 0) break;

                                fs.Write(buffer, 0, currentRead);
                                totalBytesReceived += currentRead;
                            }
                        }

                        // Bắn sự kiện lên giao diện thông báo nhận tệp thành công
                        OnFilesReceived?.Invoke($"[Hệ thống]: Đã nhận file '{fileName}' ({fileSize} bytes) từ Client {clientInfo.Name} ({ipEnd.Address}) lưu tại Desktop.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi trong quá trình bóc tách tệp tin: {ex.Message}", "Lỗi truyền file");
                    }
                }
            }

            // Giải phóng Client khỏi danh sách khi ngắt kết nối
            lock (ConnectedClients)
            {
                // ĐÃ SỬA DÒNG 202: Xóa thực thể clientInfo (Thay vì xóa client thô)
                ConnectedClients.Remove(clientInfo);
            }
            OnClientListChanged?.Invoke();
        }
    }
}