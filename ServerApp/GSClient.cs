using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerApp
{
    public partial class GSClient : Form
    {
        // Khai báo biến toàn cục để lưu lại Menu chính
        private Form _mainMenu;

        // ĐÃ SỬA: Hàm khởi tạo nhận MainForm truyền vào từ bên ngoài
        public GSClient(Form mainMenu)
        {
            InitializeComponent();
            this._mainMenu = mainMenu;
        }

        private void GSClient_Load(object sender, EventArgs e)
        {
            // Thiết lập vị trí Form hiện ra ngay chính giữa màn hình máy tính
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        // --- NÚT QUAY LẠI TRÊN GIAO DIỆN GIÁM SÁT ---
        private void btnBackToMenu_Click(object sender, EventArgs e)
        {
            // Hiện lại Menu chính ban đầu trước
            if (_mainMenu != null)
            {
                _mainMenu.Show();
            }

            // Sau đó mới đóng cửa sổ giám sát hiện tại
            this.Close();
        }

        // --- SỰ KIỆN PHÒNG HỜ NGƯỜI DÙNG BẤM DẤU [X] ĐỎ TRÊN GÓC CỬA SỔ ---
        private void GSClient_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_mainMenu != null && !_mainMenu.Visible)
            {
                _mainMenu.Show();
            }
        }

        private void lblPort_Click(object sender, EventArgs e)
        {

        }

        private void btnKick_Click(object sender, EventArgs e)
        {

        }

        private void btnBuzz_Click(object sender, EventArgs e)
        {

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {

        }
    }
}