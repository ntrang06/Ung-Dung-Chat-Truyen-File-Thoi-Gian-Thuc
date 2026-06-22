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
        private string _endPoint;

        public ClientHandler(TcpClient client, Server server)
        {
            _client = client;
            _server = server;
            _stream = client.GetStream();
            _isConnected = true;
            _endPoint = client.Client.RemoteEndPoint.ToString();
        }

        public void Process()
        {
            byte[] commandByte = new byte[1];

            while (_isConnected)
            {
                try
                {
                    int bytesRead = _stream.Read(commandByte, 0, 1);
                    if (bytesRead == 0) break;

                    byte cmd = commandByte[0];

                    if (cmd == 0x01) // Nhận tin nhắn chat từ client
                    {
                        HandleChat();
                    }
                    else if (cmd == 0x02) // Chuẩn bị nhận file từ client
                    {
                        HandleReceiveFile();
                    }
                }
                catch
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
            _server.InvokeMessageReceived($"[Client {_endPoint}]: {message}");
        }

        private void HandleReceiveFile()
        {
            // 1. Nhận thông tin tên file
            byte[] lengthBuffer = new byte[4];
            _stream.Read(lengthBuffer, 0, 4);
            int nameLength = BitConverter.ToInt32(lengthBuffer, 0);

            byte[] nameBuffer = new byte[nameLength];
            _stream.Read(nameBuffer, 0, nameLength);
            string fileName = Encoding.UTF8.GetString(nameBuffer);

            // 2. Nhận dung lượng file
            byte[] sizeBuffer = new byte[8];
            _stream.Read(sizeBuffer, 0, 8);
            long fileSize = BitConverter.ToInt64(sizeBuffer, 0);

            // Tải file xuống ổ đĩa bằng Khối đệm chia nhỏ dữ liệu ổn định
            string savePath = Path.Combine(FileManager.GetStoragePath(), fileName);
            string readableSize = (fileSize / 1024.0 / 1024.0).ToString("0.##") + " MB";

            try
            {
                using (FileStream fs = new FileStream(savePath, FileMode.Create, FileAccess.Write))
                {
                    byte[] buffer = new byte[8192]; // Buffer khối dữ liệu nhỏ tối ưu 8KB
                    long totalBytesReceived = 0;

                    while (totalBytesReceived < fileSize)
                    {
                        int bytesToRead = (int)Math.Min(buffer.Length, fileSize - totalBytesReceived);
                        int read = _stream.Read(buffer, 0, bytesToRead);

                        if (read == 0) throw new Exception("Ngắt kết nối đột ngột.");

                        fs.Write(buffer, 0, read);
                        totalBytesReceived += read;

                        int progress = (int)((totalBytesReceived * 100) / fileSize);
                        _server.InvokeProgressUpdated(progress);
                    }
                }
                _server.InvokeFileReceived(fileName, readableSize, "Thành công");
            }
            catch
            {
                _server.InvokeFileReceived(fileName, readableSize, "Thất bại/Lỗi");
            }
        }

        public void SendMessage(string message)
        {
            try
            {
                byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                byte[] lengthBytes = BitConverter.GetBytes(messageBytes.Length);

                _stream.WriteByte(0x01); // Thêm byte điều khiển chat protocol
                _stream.Write(lengthBytes, 0, 4);
                _stream.Write(messageBytes, 0, messageBytes.Length);
                _stream.Flush();
            }
            catch { }
        }

        public void Close()
        {
            if (_isConnected)
            {
                _isConnected = false;
                _stream?.Close();
                _client?.Close();
                _server.RemoveClient(this, _endPoint);
            }
        }
    }
}