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
        public event Action<string> OnLogReceived;
        public event Action<string> OnClientConnected;
        public event Action<string> OnClientDisconnected;
        public event Action<int> OnProgressUpdated;
        public event Action<string, string, string> OnFileReceived;

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
                OnLogReceived?.Invoke($"Máy chủ bắt đầu lắng nghe trên cổng {port}...");
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
                    string ep = client.Client.RemoteEndPoint.ToString();

                    OnLogReceived?.Invoke($"Client kết nối thành công: {ep}");
                    OnClientConnected?.Invoke(ep);

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

                            StreamReader reader = new StreamReader(stream, Encoding.UTF8, false, 1024, true);

                            string firstMessage = reader.ReadLine();

                            if (string.IsNullOrEmpty(firstMessage))
                                return;

                            string clientName = "Ẩn danh";

                            if (firstMessage.StartsWith("CONNECT|"))
                            {
                                clientName = firstMessage.Substring(8);
                            }

                            // Khôi phục lại trạng thái chờ vô hạn cho các gói tin Chat/File sau đó
                            client.ReceiveTimeout = 0;

                            // Tạo đối tượng ClientInfo
                            ClientInfo clientInfo = new ClientInfo(client, clientName);

                            lock (ConnectedClients)
                            {
                                ConnectedClients.Add(clientInfo);
                            }

                            OnClientListChanged?.Invoke();
                            // Bắt đầu xử lý dữ liệu từ client (chat, upload...)
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

            while (_isRunning && client.Connected) 
            {
                try
                {
                    int cmd = stream.ReadByte();

                    if (cmd == -1)
                        break;

                    //-----------------------------------
                    // CHAT
                    //-----------------------------------
                    if (cmd == 0x01)
                    {
                        byte[] lenBuffer = new byte[4];
                        ReadFull(stream, lenBuffer, 4);

                        int msgLength = BitConverter.ToInt32(lenBuffer, 0);

                        byte[] msgBuffer = new byte[msgLength];
                        ReadFull(stream, msgBuffer, msgLength);

                        string message = Encoding.UTF8.GetString(msgBuffer);

                        IPEndPoint ip = (IPEndPoint)client.Client.RemoteEndPoint;

                        string fullMessage =
                            $"[{DateTime.Now:HH:mm:ss}] [{clientInfo.Name} - {ip.Address}]:" +
                            Environment.NewLine +
                            message;

                        OnChatReceived?.Invoke(fullMessage);

                        BroadcastMessage(fullMessage);
                    }

                    //-----------------------------------
                    // FILE
                    //-----------------------------------
                    else if (cmd == 0x02)
                    {
                        MessageBox.Show("Server nhận lệnh Upload");
                        byte[] lenBuffer = new byte[4];
                        ReadFull(stream, lenBuffer, 4);

                        int nameLength = BitConverter.ToInt32(lenBuffer, 0);

                        byte[] nameBuffer = new byte[nameLength];
                        ReadFull(stream, nameBuffer, nameLength);

                        string fileName = Encoding.UTF8.GetString(nameBuffer);

                        byte[] sizeBuffer = new byte[8];
                        ReadFull(stream, sizeBuffer, 8);

                        long fileSize = BitConverter.ToInt64(sizeBuffer, 0);

                        string folder = Path.Combine(Application.StartupPath, "ServerFiles");

                        if (!Directory.Exists(folder))
                            Directory.CreateDirectory(folder);
                        string savePath = Path.Combine(folder, fileName);
                        MessageBox.Show(savePath);

                        using (FileStream fs = new FileStream(savePath, FileMode.Create))
                        {
                            byte[] buffer = new byte[8192];

                            long total = 0;

                            while (total < fileSize)
                            {
                                int read = stream.Read(
                                    buffer,
                                    0,
                                    (int)Math.Min(buffer.Length, fileSize - total));

                                if (read == 0)
                                    break;

                                fs.Write(buffer, 0, read);

                                total += read;
                            }
                        }

                        OnFilesReceived?.Invoke(
                            $"{fileName}*{fileSize / 1024} KB*{Path.GetExtension(fileName)}");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    break;
                }
            }

            lock (ConnectedClients)
            {
                ConnectedClients.Remove(clientInfo);
                OnClientDisconnected?.Invoke(clientInfo.Socket.Client.RemoteEndPoint.ToString());

                OnLogReceived?.Invoke($"Client {clientInfo.Socket.Client.RemoteEndPoint} đã ngắt kết nối.");
            }

            OnClientListChanged?.Invoke();
        }
        private void ReadFull(NetworkStream stream, byte[] buffer, int size)
        {
            int offset = 0;

            while (offset < size)
            {
                int read = stream.Read(buffer, offset, size - offset);

                if (read == 0)
                    throw new Exception("Client disconnected");

                offset += read;
            }
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
        public void SendFile(ClientInfo client, string filePath)
        {
            try
            {
                NetworkStream stream = client.Socket.GetStream();

                byte[] fileData = File.ReadAllBytes(filePath);

                string fileName = Path.GetFileName(filePath);

                byte[] nameBytes = Encoding.UTF8.GetBytes(fileName);

                stream.WriteByte(0x02);

                stream.Write(BitConverter.GetBytes(nameBytes.Length), 0, 4);

                stream.Write(nameBytes, 0, nameBytes.Length);

                stream.Write(BitConverter.GetBytes((long)fileData.Length), 0, 8);

                stream.Write(fileData, 0, fileData.Length);

                stream.Flush();
            }
            catch
            {

            }

        }
        public void SendCommand(ClientInfo client, string command)
        {
            try
            {
                NetworkStream stream = client.Socket.GetStream();

                byte[] data = Encoding.UTF8.GetBytes(command + "\n");

                stream.Write(data, 0, data.Length);
                stream.Flush();
            }
            catch
            {
            }
        }
    }
}


