using System;
using System.Windows.Forms;

namespace ServerApp
{
    public partial class ServerForm : Form
    {
        public ServerForm()
        {
            InitializeComponent();
        }

        // --- CODE CHO NÚT KHỞI ĐỘNG ---
        // Lưu ý: Nếu ở Bước 1 máy tự sinh ra tên hàm có đuôi _1 (Ví dụ: btnStartServer_Click_1)
        // Thì bạn chỉ cần đổi tên hàm dưới đây thành btnStartServer_Click_1 cho khớp nhé.
        private void btnKhoiDongServer_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra dữ liệu ô nhập liệu (Đảm bảo ô TextBox tên là txtPort)
            string portInput = txtPort.Text.Trim();
            if (string.IsNullOrEmpty(portInput))
            {
                MessageBox.Show("Vui lòng nhập số Cổng (Port) trước khi khởi động!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Ép kiểu chữ sang số nguyên int
            if (int.TryParse(portInput, out int port))
            {
                // Kiểm tra dải Port an toàn hệ thống (thường từ 1024 - 65535)
                if (port < 1024 || port > 65535)
                {
                    MessageBox.Show("Vui lòng nhập Port trong khoảng an toàn từ 1024 đến 65535!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 3. Khởi tạo MainForm và truyền Port sang cho Menu chính
                MainForm mainApp = new MainForm(port, this);

                // 4. Mở MainForm lên và ẩn ServerForm hiện tại đi
                mainApp.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Cổng kết nối (Port) bắt buộc phải là ký tự số nguyên!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- CODE CHO NÚT DỪNG ---
        // Tương tự, nếu máy tự sinh ra tên btnStopServer_Click_1 thì bạn đổi tên hàm dưới đây theo nó nhé.
        private void btnDungServer_Click(object sender, EventArgs e)
        {
            // Đóng hoàn toàn toàn bộ tiến trình ứng dụng
            Application.Exit();
        }

   
    }
}