using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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
        public event Action<string> OnCommandReceived;
        public event Action<string> OnChatReceived;
        public event Action<string> OnFileReceived;
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
                byte[] connectData = Encoding.UTF8.GetBytes($"CONNECT|{clientName}\n");
                _stream.Write(connectData, 0, connectData.Length);
                _stream.Flush();
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
            try
            {
                while (_isConnected && _socket.Connected)
                {
                    int header = _stream.ReadByte();

                    if (header == -1)
                        break;

                    if (header == 0x01)
                    {
                        byte[] len = new byte[4];
                        ReadFull(len, 4);

                        int msgLen = BitConverter.ToInt32(len, 0);

                        byte[] data = new byte[msgLen];
                        ReadFull(data, msgLen);

                        string msg = Encoding.UTF8.GetString(data);

                        OnChatReceived?.Invoke(msg);
                    }
                    else if (header == 0x02)
                    {
                        byte[] nameLen = new byte[4];
                        ReadFull(nameLen, 4);

                        int lenName = BitConverter.ToInt32(nameLen, 0);

                        byte[] nameBytes = new byte[lenName];
                        ReadFull(nameBytes, lenName);

                        string fileName = Encoding.UTF8.GetString(nameBytes);

                        byte[] sizeBytes = new byte[8];
                        ReadFull(sizeBytes, 8);

                        long fileSize = BitConverter.ToInt64(sizeBytes, 0);

                        byte[] fileData = new byte[fileSize];
                        ReadFull(fileData, (int)fileSize);

                        File.WriteAllBytes(fileName, fileData);

                        OnFileReceived?.Invoke(fileName);
                    }
                }
            }
            catch
            {
                HandleDisconnect();
            }
        }
        private void ReadFull(byte[] buffer, int size)
        {
            int offset = 0;

            while (offset < size)
            {
                int read = _stream.Read(buffer, offset, size - offset);

                if (read == 0)
                    throw new Exception();

                offset += read;
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
                Form frm = Application.OpenForms[0];

                if (frm.IsHandleCreated)
                {
                    frm.BeginInvoke(new MethodInvoker(() =>
                    {
                        MessageBox.Show(
                            "Mất kết nối tới Server! Ứng dụng sẽ tự động quay về màn hình đăng nhập.",
                            "Thông báo ngắt kết nối",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);

                        Application.Restart();
                    }));
                }
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