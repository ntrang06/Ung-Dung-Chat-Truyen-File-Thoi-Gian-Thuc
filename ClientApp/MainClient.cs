using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.ActivationContext;

namespace ClientApp
{
    public partial class MainClient : Form
    {
        private Form _connectForm;

        public MainClient(Form connectForm)
        {
            InitializeComponent();
            this._connectForm = connectForm;
        }

        private void MainClient_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            // Không cần gọi luồng mạng ở đây nữa!
        }

        private void btnBackToConfig_Click(object sender, EventArgs e)
        {
            SocketClient.Instance.Disconnect(); // Ngắt kết nối khi chủ động quay lại
            this.Close();
            if (_connectForm != null) _connectForm.Show();
        }

        private void MainClient_FormClosing(object sender, FormClosingEventArgs e)
        {
            SocketClient.Instance.Disconnect();
            if (_connectForm != null) _connectForm.Close();
        }
    }
}