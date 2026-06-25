using System;
using System.Windows.Forms;

namespace ServerApp
{
    public partial class MainForm : Form
    {
        private int _port;
        private Form _configForm;

        // Hàm khởi tạo nhận 2 đối số từ ServerForm nhập Port truyền sang
        public MainForm(int port, Form configForm)
        {
            InitializeComponent();
            this._port = port;
            this._configForm = configForm;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Form mở lên trống trải, gọn gàng và hiện nguyên vẹn 3 nút Menu ban đầu
        }

        // --- KHI BẤM NÚT KHUNG CHAT HỆ THỐNG: TỰ ĐỘNG BẬT CỬA SỔ MỚI ---
        private void btnKhungChat_Click(object sender, EventArgs e)
        {
            // Khởi tạo trực tiếp cửa sổ ChatServer
            ChatServer fChat = new ChatServer(this);
            this.Hide();

            // Bật bung nó lên thành một Form độc lập hiển thị trên màn hình
            fChat.Show();
        }

        private void btnGiamSat_Click(object sender, EventArgs e)
        {
            // Sau này làm chức năng Giám sát bạn chỉ cần gọi tương tự:
            // FormGiamSat f = new FormGiamSat(); f.Show();
        }

        private void btnQuanLyFile_Click(object sender, EventArgs e)
        {
            // Sau này làm chức năng Quản lý file bạn cũng gọi lên Form riêng như vậy
        }

        // --- NÚT QUAY LẠI TRÊN MENU (TRỞ VỀ FORM NHẬP PORT BAN ĐẦU) ---
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Dispose(); // Giải phóng MainForm hiện tại
            if (_configForm != null)
            {
                _configForm.Show(); // Hiện lại ServerForm nhập Port ban đầu
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit(); // Tắt hết toàn bộ ứng dụng chạy ngầm khi đóng Form chính
        }

        private void btnBackToConfig_Click(object sender, EventArgs e)
        {
            // Nút này trùng chức năng với btnBack, gọi lại hàm btnBack cho gọn
            btnBack_Click(sender, e);
        }
        public void QuayVeMenu()
        {                
        }
    }
}