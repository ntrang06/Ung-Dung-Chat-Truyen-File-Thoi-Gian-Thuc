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
            lstChat chat = new lstChat();
            chat.ShowDialog();
        }

        // ================= Truyền File =================
        private void btnFile_Click(object sender, EventArgs e)
        {
            ClientFileForm file = new ClientFileForm();
            file.ShowDialog();
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

        private void btnChat_Click_1(object sender, EventArgs e)
        {
           
        }
    }
}