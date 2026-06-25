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
    public partial class ChatServer : Form
    {
        // VỊ TRÍ QUAN TRỌNG: Phải khai báo biến này ở đây để toàn bộ các hàm bên dưới dùng chung
        private Form _mainMenu;

        // Hàm khởi tạo nhận MainForm truyền vào từ bên ngoài
        public ChatServer(Form mainMenu)
        {
            InitializeComponent();
            this._mainMenu = mainMenu; // Lưu lại Menu chính vào biến toàn cục
        }

        // --- NÚT QUAY LẠI TRÊN GIAO DIỆN KHUNG CHAT ---
        private void btnBackToMenu_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra nếu có Menu chính thì bắt nó hiện lên trước
            if (_mainMenu != null)
            {
                _mainMenu.Show();
            }

            // 2. Sau đó mới đóng cửa sổ chat hiện tại
            this.Close();
        }

        // --- SỰ KIỆN ĐỀ PHÒNG NGƯỜI DÙNG BẤM DẤU [X] ĐỎ THAY VÌ BẤM NÚT QUAY LẠI ---
        private void ChatServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Nếu tắt bằng dấu X và Menu chính đang ẩn thì hiện Menu lên lại
            if (_mainMenu != null && !_mainMenu.Visible)
            {
                _mainMenu.Show();
            }
        }

        // --- NÚT GỬI TIN NHẮN ---
        private void btnSend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtInput.Text.Trim())) return;

            string msg = txtInput.Text.Trim();

            // Hiển thị tin nhắn của Server lên ô lịch sử lớn
            HienThiTinNhan($"[Server]: {msg}");

            // Xóa trống ô nhập để sẵn sàng gõ tin tiếp theo
            txtInput.Clear();
            txtInput.Focus();
        }

        private void ChatServer_Load(object sender, EventArgs e)
        {
            // Để trống hoặc viết cấu hình Socket khi load Form tại đây
        }

        // Hàm hỗ trợ hiển thị tin nhắn từ các luồng khác nhau
        public void HienThiTinNhan(string message)
        {
            if (txtChatHistory.InvokeRequired)
            {
                txtChatHistory.Invoke(new Action(() => HienThiTinNhan(message)));
            }
            else
            {
                txtChatHistory.AppendText(message + Environment.NewLine);
            }
        }
    }
}