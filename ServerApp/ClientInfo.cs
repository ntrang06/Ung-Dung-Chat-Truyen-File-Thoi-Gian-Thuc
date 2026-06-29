using System;
using System.Net;
using System.Net.Sockets;

namespace ServerApp
{
    public class ClientInfo
    {
        // Socket kết nối
        public TcpClient Socket { get; set; }

        // Tên Client
        public string Name { get; set; }

        // Thời gian kết nối
        public DateTime LoginTime { get; set; }

        // Luồng mạng
        public NetworkStream Stream
        {
            get
            {
                if (Socket != null && Socket.Connected)
                    return Socket.GetStream();

                return null;
            }
        }

        // Địa chỉ IP
        public string IP
        {
            get
            {
                if (Socket != null && Socket.Connected)
                {
                    return ((IPEndPoint)Socket.Client.RemoteEndPoint).Address.ToString();
                }

                return "Unknown";
            }
        }

        // Port
        public int Port
        {
            get
            {
                if (Socket != null && Socket.Connected)
                {
                    return ((IPEndPoint)Socket.Client.RemoteEndPoint).Port;
                }

                return 0;
            }
        }

        public ClientInfo(TcpClient socket, string name)
        {
            Socket = socket;
            Name = name;
            LoginTime = DateTime.Now;
        }

        // Đóng kết nối
        public void Close()
        {
            try
            {
                Stream?.Close();
            }
            catch { }

            try
            {
                Socket?.Close();
            }
            catch { }
        }

        public override string ToString()
        {
            if (Socket != null && Socket.Connected)
            {
                return $"{Name} ({IP}:{Port})";
            }

            return $"{Name} (Offline)";
        }
    }
}