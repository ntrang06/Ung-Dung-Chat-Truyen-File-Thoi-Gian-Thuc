using System;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace ClientApp
{
    public partial class ClientForm : Form
    {
        // Biến static để các Form khác bên Client (Chat, QLFile) có thể dùng chung luồng kết nối này
        public static TcpClient ClientSocket { get; private set; }

        public ClientForm()
        {
            InitializeComponent();
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        private void btnConnect_Click(object sender, EventArgs e)
        {
            string ip = txtIP.Text.Trim();
            string portStr = txtPort.Text.Trim();

            if (string.IsNullOrEmpty(ip) || string.IsNullOrEmpty(portStr))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Địa chỉ IP và Cổng kết nối!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(portStr, out int port))
            {
                MessageBox.Show("Cổng kết nối phải là ký tự số!", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Khởi tạo kết nối Socket tới Server
                ClientSocket = new TcpClient();
                ClientSocket.Connect(ip, port);

                MessageBox.Show("Kết nối tới Server thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide(); // Ẩn Form kết nối hiện tại đi

                // Tạo và hiển thị Form Menu chính của Client (Truyền Form kết nối vào để quản lý)
              
                MainClient mainMenu = new MainClient(this);
                mainMenu.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể kết nối tới Server! Đảm bảo Server đã bật và mở đúng Port.\nChi tiết lỗi: {ex.Message}", "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ClientSocket = null;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}