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
        public static string ClientName { get; private set; }

        public ClientForm()
        {
            InitializeComponent();
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            txtIP.Text = ""; // Gợi ý sẵn IP cục bộ để test cho nhanh
            txtPort.Text = "";    // Gợi ý sẵn Port
            txtClientName.Text = "";
        }

        // CHỈ DÙNG DUY NHẤT MỘT HÀM NÀY CHO NÚT KẾT NỐI
        private void btnConnect_Click(object sender, EventArgs e)
        {
            string ip = txtIP.Text.Trim();
            string portStr = txtPort.Text.Trim();
            string clientName = txtClientName.Text.Trim();

            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrEmpty(ip) || string.IsNullOrEmpty(portStr) || string.IsNullOrEmpty(clientName))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Địa chỉ IP, Cổng và Tên người dùng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                // Lưu lại tên người dùng vào biến static
                ClientName = clientName;
                NetworkStream stream = ClientSocket.GetStream();

                // Gửi gói tin định danh kết nối sang cho Server
                byte[] connectData = Encoding.UTF8.GetBytes($"CONNECT|{clientName}");
                stream.Write(connectData, 0, connectData.Length);
                stream.Flush(); // Đẩy dữ liệu đi ngay lập tức

                MessageBox.Show("Kết nối tới Server thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide(); // Ẩn Form kết nối hiện tại đi

                // Tạo và hiển thị Form Menu chính của Client
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