using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace ServerApp
{
    public class ClientInfo
    {
        // Lưu giữ kết nối mạng của máy đó
        public TcpClient Socket { get; set; }

        // Lưu giữ tên người dùng (bsd, dac, fadv...)
        public string Name { get; set; }

        // Lưu giữ thời điểm máy này kết nối vào Server
        public DateTime LoginTime { get; set; } = DateTime.Now; // Tự động lấy thời gian lúc kết nối

        // Hàm khởi tạo (Constructor) để nạp dữ liệu nhanh
        public ClientInfo(TcpClient socket, string name)
        {
            this.Socket = socket;
            this.Name = name;
            this.LoginTime = DateTime.Now;
        }
        public override string ToString()
        {
            if (this.Socket != null && this.Socket.Connected)
            {
                var ipEnd = (System.Net.IPEndPoint)this.Socket.Client.RemoteEndPoint;
                return $"{Name} ({ipEnd.Address}:{ipEnd.Port})";
            }
            return $"{Name} (Offline)";
        }
    }
}
