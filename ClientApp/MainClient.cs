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
        }

        private void btnBackToConfig_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose(); // Giải phóng MainForm hiện tại
            if (_connectForm != null)
            {
                _connectForm.Show(); // Hiện lại ServerForm nhập Port ban đầu
            }
        }
        private void MainClient_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_connectForm != null)
            {
                _connectForm.Close(); // Đóng Form kết nối ngầm sẽ giải phóng toàn bộ RAM Client
            }
        }
    }
}
