using System;
using System.Windows.Forms;

namespace ClientApp
{
    public partial class MainClient : Form
    {
        private Form _connectForm;

        public MainClient(Form connectForm)
        {
            InitializeComponent();
            _connectForm = connectForm;
        }

        private void MainClient_Load(object sender, EventArgs e)
        {
            StartPosition = FormStartPosition.CenterScreen;
        }

        // ================= Quay lại =================
        private void btnBackToConfig_Click(object sender, EventArgs e)
        {
            SocketClient.Instance.Disconnect();

            this.Hide();

            if (_connectForm != null)
                _connectForm.Show();
        }

        // ================= Chat =================
        private void btnChat_Click(object sender, EventArgs e)
        {
            // BẮT BUỘC: Truyền 'this' (chính MainClient) vào để form chat có thể điều khiển quay lại
            lstChat chat = new lstChat(this);

            this.Hide();
            chat.ShowDialog(); // Chạy Form chat dưới dạng Dialog chặn
            this.Show();       // Tự động hiển thị lại Menu chính ngay khi tắt Form chat
        }

        // ================= Truyền File =================
        private void btnFile_Click(object sender, EventArgs e)
        {
            ClientFileForm file = new ClientFileForm();

            this.Hide();
            file.ShowDialog();
            this.Show(); // Hiển thị lại Menu chính khi tắt Form truyền file
        }

        // ================= Thoát =================
        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult rs = MessageBox.Show(
        "Do you want to exit?",
        "Exit",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Question);

            if (rs == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        // ================= Đóng Form =================
        private void MainClient_FormClosing(object sender, FormClosingEventArgs e)
        {
            SocketClient.Instance.Disconnect();
            Application.Exit();
        }
    }
}