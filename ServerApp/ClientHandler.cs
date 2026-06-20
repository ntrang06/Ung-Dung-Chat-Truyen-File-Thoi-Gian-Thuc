using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace ServerApp
{
    public class ClientHandler
    {
        private TcpClient _client;
        private Server _server;
        private NetworkStream _stream;
        private bool _isConnected;

        public ClientHandler(TcpClient client, Server server)
        {
            _client = client;
            _server = server;
            _stream = client.GetStream();
            _isConnected = true;
        }

        public void Process()
        {
            byte[] commandByte = new byte[1];

            while (_isConnected)
            {
                try
                {
                    // Đọc 1 byte đầu tiên để nhận dạng gói dữ liệu (Mã lệnh protocol)
                    int bytesRead = _stream.Read(commandByte, 0, 1);
                    if (bytesRead == 0) break;

                    byte cmd = commandByte[0];

                    if (cmd == 0x01) // 0x01 đại diện cho gói tin nhắn Chat
                    {
                        HandleChat();
                    }
                    else if (cmd == 0x02) // 0x02 đại diện cho gói chuẩn bị truyền dữ liệu File
                    {
                        HandleReceiveFile();
                    }
                }
                catch (Exception)
                {
                    break;
                }
            }
            Close();
        }

        private void HandleChat()
        {
            byte[] lengthBuffer = new byte[4];
            _stream.Read(lengthBuffer, 0, 4);
            int messageLength = BitConverter.ToInt32(lengthBuffer, 0);

            byte[] messageBuffer = new byte[messageLength];
            int totalRead = 0;
            while (totalRead < messageLength)
            {
                int read = _stream.Read(messageBuffer, totalRead, messageLength - totalRead);
                totalRead += read;
            }

            string message = Encoding.UTF8.GetString(messageBuffer);
            _server.UpdateProgress(0); // Reset progress bar về 0 khi chat
            // In tin nhắn ra Log hệ thống của Form1
            _server.Stop(); // Có thể đổi thành hàm in log tùy ý:
            // _server.InvokeLog($"[Chat] Client: {message}");
        }

        private void HandleReceiveFile()
        {
            // 1. Đọc độ dài tên file và nội dung tên file
            byte[] lengthBuffer = new byte[4];
            _stream.Read(lengthBuffer, 0, 4);
            int nameLength = BitConverter.ToInt32(lengthBuffer, 0);

            byte[] nameBuffer = new byte[nameLength];
            _stream.Read(nameBuffer, 0, nameLength);
            string fileName = Encoding.UTF8.GetString(nameBuffer);

            // 2. Đọc dung lượng tổng của file (8 bytes)
            byte[] sizeBuffer = new byte[8];
            _stream.Read(sizeBuffer, 0, 8);
            long fileSize = BitConverter.ToInt64(sizeBuffer, 0);

            // 3. Tiến hành ghi file vào ổ đĩa thông qua cơ chế Buffer 8KB từng khối dữ liệu liên tiếp
            string savePath = Path.Combine(FileManager.GetStoragePath(), fileName);

            using (FileStream fs = new FileStream(savePath, FileMode.Create, FileAccess.Write))
            {
                byte[] buffer = new byte[8192];
                long totalBytesReceived = 0;

                while (totalBytesReceived < fileSize)
                {
                    int bytesToRead = (int)Math.Min(buffer.Length, fileSize - totalBytesReceived);
                    int read = _stream.Read(buffer, 0, bytesToRead);

                    if (read == 0) throw new Exception("Kết nối bị gián đoạn giữa chừng.");

                    fs.Write(buffer, 0, read);
                    totalBytesReceived += read;

                    // Tính toán phần trăm và gửi ra giao diện hiển thị ProgressBar
                    int progress = (int)((totalBytesReceived * 100) / fileSize);
                    _server.UpdateProgress(progress);
                }
            }
        }

        public void Close()
        {
            if (_isConnected)
            {
                _isConnected = false;
                _stream?.Close();
                _client?.Close();
                _server.RemoveClient(this);
            }
        }
    }
}