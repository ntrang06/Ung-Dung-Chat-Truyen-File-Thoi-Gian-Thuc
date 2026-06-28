using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientApp
{
    public class SocketClient
    {
        // Khởi tạo cơ chế Singleton để gọi ở mọi Form: SocketClient.Instance
        private static SocketClient _instance;
        public static SocketClient Instance => _instance ?? (_instance = new SocketClient());

        private TcpClient _socket;
        private NetworkStream _stream;
        private bool _isConnected = false;

        // Định nghĩa các sự kiện (Event) để các Giao diện Form đăng ký hứng khi có tin về
        public event Action<string> OnBuzzReceived;
        public event Action OnServerDisconnected;

        private SocketClient() { }

        // Hàm thực hiện kết nối tới Server
        public bool Connect(string ip, int port, string clientName)
        {
            try
            {
                _socket = new TcpClient();
                _socket.Connect(ip, port);
                _stream = _socket.GetStream();
                _isConnected = true;

                // Gửi gói tin định danh sang Server
                byte[] connectData = Encoding.UTF8.GetBytes($"CONNECT|{clientName}");
                _stream.Write(connectData, 0, connectData.Length);
                _stream.Flush();

                // Kích hoạt luồng ngầm liên tục ngồi đợi "nhặt" dữ liệu từ Server gửi về
                Task.Run(() => ListenToServer());

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kết nối Socket: {ex.Message}");
                Disconnect();
                return false;
            }
        }

        // Luồng chạy ngầm đọc dữ liệu xuyên suốt ứng dụng
        private void ListenToServer()
        {
            byte[] buffer = new byte[1024];

            try
            {
                while (_isConnected && _socket != null && _socket.Connected)
                {
                    int bytesRead = _stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) // Server chủ động ngắt kết nối
                    {
                        HandleDisconnect();
                        break;
                    }

                    string receivedText = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    // 1. XỬ LÝ LỆNH CẢNH BÁO GIÁM SÁT (BUZZ)
                    if (receivedText.StartsWith("BUZZ|"))
                    {
                        string alertContent = receivedText.Substring(5);

                        // Kích hoạt sự kiện toàn hệ thống, hoặc hiển thị MessageBox luôn tại đây cho an toàn
                        if (Application.OpenForms.Count > 0)
                        {
                            Application.OpenForms[0].Invoke(new MethodInvoker(() =>
                            {
                                MessageBox.Show(alertContent, "CẢNH BÁO TỪ HỆ THỐNG GIÁM SÁT", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }));
                        }

                        // Bắn sự kiện ra ngoài nếu các Form con khác muốn bắt thêm logic
                        OnBuzzReceived?.Invoke(alertContent);
                        continue;
                    }

                    // Bạn có thể xử lý thêm các gói tin Chat, File... ở các điều kiện else if tiếp theo tại đây
                }
            }
            catch
            {
                HandleDisconnect();
            }
        }

        // Xử lý khi mất kết nối đột ngột
        private void HandleDisconnect()
        {
            if (!_isConnected) return;

            Disconnect();

            // Hiển thị thông báo ép buộc dù đang ở bất kỳ giao diện nào
            if (Application.OpenForms.Count > 0)
            {
                Application.OpenForms[0].Invoke(new MethodInvoker(() =>
                {
                    MessageBox.Show("Mất kết nối tới Server! Ứng dụng sẽ tự động quay về màn hình đăng nhập.", "Thông báo ngắt kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    // Code tự động dọn dẹp và bật lại Form Đăng nhập ban đầu nếu muốn
                    Application.Restart();
                }));
            }

            OnServerDisconnected?.Invoke();
        }

        // Đóng và giải phóng kết nối
        public void Disconnect()
        {
            _isConnected = false;
            try { _stream?.Close(); } catch { }
            try { _socket?.Close(); } catch { }
            _stream = null;
            _socket = null;
        }

        // Hàm tiện ích giúp các Form con gửi dữ liệu lên Server cực nhanh
        public void SendData(string text)
        {
            try
            {
                if (_socket != null && _socket.Connected && _stream != null)
                {
                    byte[] data = Encoding.UTF8.GetBytes(text);
                    _stream.Write(data, 0, data.Length);
                    _stream.Flush();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi gửi dữ liệu: {ex.Message}");
            }
        }
    }
}