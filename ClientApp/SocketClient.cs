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

        public TcpClient Client => _socket;

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
            byte[] buffer = new byte[8192];

            try
            {
                while (_isConnected && _socket != null && _socket.Connected)
                {
                    int bytesRead = _stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                    {
                        HandleDisconnect();
                        break;
                    }

                    string receivedText = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    // Debug xem Server thực sự gửi gì
                    System.Diagnostics.Debug.WriteLine("Server gửi: " + receivedText);

                    if (receivedText.StartsWith("BUZZ|"))
                    {
                        string alertContent = receivedText.Substring(5);

                        if (Application.OpenForms.Count > 0)
                        {
                            Application.OpenForms[0].Invoke(new MethodInvoker(() =>
                            {
                                MessageBox.Show(alertContent,
                                    "CẢNH BÁO TỪ HỆ THỐNG",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                            }));
                        }

                        OnBuzzReceived?.Invoke(alertContent);
                    }
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
                if (_socket == null || !_socket.Connected || _stream == null)
                    return;

                byte[] data = Encoding.UTF8.GetBytes(text);

                _stream.Write(data, 0, data.Length);
                _stream.Flush();
                MessageBox.Show("Đã gửi: " + text);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        public NetworkStream GetStream()
        {
            return _stream;
        }
    }
}