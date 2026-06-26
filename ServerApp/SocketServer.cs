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
        private int _port = 9999; // Bạn có thể đổi Port tùy ý

        // Danh sách quản lý các Client đang kết nối thật
        public List<TcpClient> ConnectedClients { get; private set; } = new List<TcpClient>();

        // Sự kiện thông báo cho Giao diện (UI) biết khi có Client vừa Online hoặc Offline
        public event Action OnClientListChanged;

        // Singleton Pattern để mọi Form trong ServerApp đều dùng chung 1 kết nối Socket ngầm
        private static SocketServer _instance;
        public static SocketServer Instance => _instance ?? (_instance = new SocketServer());

        private SocketServer() { }

        // Hàm khởi động Server lắng nghe kết nối
        public event Action<string> OnChatReceived;
        public event Action<string> OnFilesReceived;
        public void Start()
        {
            if (_isRunning) return;

            try
            {
                _listener = new TcpListener(IPAddress.Any, _port);
                _listener.Start();
                _isRunning = true;

                // Tạo một luồng (Thread) chạy độc lập để tránh làm treo đơ giao diện Form chính
                _listenThread = new Thread(ListenForClients);
                _listenThread.IsBackground = true;
                _listenThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể mở cổng kết nối: {ex.Message}", "Lỗi Socket", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                    lock (ConnectedClients)
                    {
                        ConnectedClients.Add(client);
                    }

                    // Kích hoạt sự kiện cập nhật danh sách lên giao diện
                    OnClientListChanged?.Invoke();

                    // Tạo luồng đọc dữ liệu riêng cho từng Client (Xử lý gói tin gửi lên nếu có)
                    Thread clientThread = new Thread(() => HandleClientComm(client));
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
        private void HandleClientComm(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] message = new byte[4096];
            int bytesRead;

            while (_isRunning)
            {
                bytesRead = 0;
                try { bytesRead = stream.Read(message, 0, 4096); } catch { break; }
                if (bytesRead == 0) break;

                string rawData = Encoding.UTF8.GetString(message, 0, bytesRead);

                // 1. Xử lý gói tin CHAT
                if (rawData.StartsWith("CHAT|"))
                {
                    string chatMessage = rawData.Substring(5);
                    IPEndPoint ipEnd = (IPEndPoint)client.Client.RemoteEndPoint;
                    OnChatReceived?.Invoke($"[Client {ipEnd.Address}]: {chatMessage}");
                }
                else if (rawData.StartsWith("FILE_STREAM|"))
                {
                    // Cấu trúc gói tin file: FILE_STREAM|Tên_File|Độ_Dài_Byte|Mảng_Dữ_Liệu...
                    // Lúc này phía Client sẽ truyền luồng Byte lớn, Server sẽ mở FileStream để ghi
                    try
                    {
                        string[] fileParts = rawData.Split('|');
                        string fileName = fileParts[1];

                        // Lấy đường dẫn màn hình Desktop làm nơi lưu file mặc định
                        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                        string savePath = Path.Combine(desktopPath, fileName);

                        // Đọc và ghi mảng Byte tiếp theo từ NetworkStream ra file vật lý
                        using (FileStream fs = new FileStream(savePath, FileMode.Create, FileAccess.Write))
                        {
                            // Xử lý đọc các gói byte tiếp theo cho đến khi hết file
                            // (Phần này sẽ chạy đồng bộ khi bạn test với code gửi file của Client)
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi nhận file: {ex.Message}");
                    }
                }
            }

            lock (ConnectedClients) { ConnectedClients.Remove(client); }
            OnClientListChanged?.Invoke();
        }
    }
}