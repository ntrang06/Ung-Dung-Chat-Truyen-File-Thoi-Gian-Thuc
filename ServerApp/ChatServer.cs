using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
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
            SocketServer.Instance.OnChatReceived -= AppendChatMessage;
            // Nếu tắt bằng dấu X và Menu chính đang ẩn thì hiện Menu lên lại
            if (_mainMenu != null && !_mainMenu.Visible)
            {
                _mainMenu.Show();
            }
        }

        // --- NÚT GỬI TIN NHẮN ---
        private void btnSend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtInput.Text)) return;

            string msgToSend = txtInput.Text;

            // Định dạng gói tin chat theo chuẩn: CHAT|Nội dung
            byte[] data = Encoding.UTF8.GetBytes("CHAT|" + msgToSend);

            bool splitSend = false;

            // Duyệt qua tất cả các Client đang online để gửi tin nhắn của Server tới toàn bộ các máy
            lock (SocketServer.Instance.ConnectedClients)
            {
                foreach (var client in SocketServer.Instance.ConnectedClients)
                {
                    if (client.Connected)
                    {
                        try
                        {
                            NetworkStream stream = client.GetStream();
                            stream.Write(data, 0, data.Length);
                            splitSend = true;
                        }
                        catch { }
                    }
                }
            }

            if (splitSend)
            {
                // Hiển thị tin nhắn chính mình vừa gửi lên Khung chat dòng chữ [Server]
                txtChatHistory.AppendText($"[Server]: {msgToSend}" + Environment.NewLine);

                // Xóa trống ô nhập tin nhắn để chuẩn bị gõ câu tiếp theo
                txtChatHistory.Clear();
                txtChatHistory.Focus();
            }
            else
            {
                MessageBox.Show("Hiện tại không có máy Client nào kết nối để nhận tin nhắn!", "Thông báo");
            }
        }

        private void ChatServer_Load(object sender, EventArgs e)
        {
            SocketServer.Instance.OnChatReceived += AppendChatMessage;
            // Để trống hoặc viết cấu hình Socket khi load Form tại đây
        }
        private void AppendChatMessage(string message)
        {
            // Tránh lỗi xung đột luồng giao diện (Cross-thread)
            if (txtChatHistory.InvokeRequired)
            {
                txtChatHistory.Invoke(new Action<string>(AppendChatMessage), message);
                return;
            }

            // Thêm tin nhắn mới vào khung hiển thị nội dung chat và xuống dòng
            txtChatHistory.AppendText(message + Environment.NewLine);
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