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

            if (string.IsNullOrEmpty(ip) || string.IsNullOrEmpty(portStr) || string.IsNullOrEmpty(clientName))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (int.TryParse(portStr, out int port))
            {
                // GỌI QUA SINGLETON TRUNG TÂM MỚI TẠO
                bool isSuccess = SocketClient.Instance.Connect(ip, port, clientName);

                if (isSuccess)
                {
                    MessageBox.Show("Kết nối tới Server thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();

                    MainClient mainMenu = new MainClient(this);
                    mainMenu.Show();
                }
            }
            else
            {
                MessageBox.Show("Port phải là số nguyên!");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}