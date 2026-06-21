using System;
using System.Windows.Forms;

namespace ServerApp
{
    public partial class Form1 : Form
    {
        private Server _server;

        public Form1()
        {
            InitializeComponent();
            _server = new Server();

            // Đăng ký sự kiện nhận Log và Tiến trình truyền tải từ Socket để đẩy lên UI
            _server.OnLogReceived += UpdateLog;
            _server.OnProgressUpdated += UpdateProgressBar;
        }

        // Sự kiện khi nhấn nút "Khởi động Server"
        private void btnStartServer_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtPort.Text, out int port))
            {
                _server.Start(port);
                btnKhoiDongServer.Enabled = false;
                btnDungServer.Enabled = true;
                txtPort.Enabled = false;
            }
            else
            {
                MessageBox.Show("Vui lòng nhập cổng (Port) hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Sự kiện khi nhấn nút "Dừng Server"
        private void btnStopServer_Click(object sender, EventArgs e)
        {
            _server.Stop();
            UpdateLog("Server đã dừng hoạt động.");
            btnKhoiDongServer.Enabled = true;
            btnDungServer.Enabled = false;
            txtPort.Enabled = true;
        }

        // Hàm cập nhật Nhật ký hệ thống (Log) an toàn từ luồng phụ (Thread-Safe)
        private void UpdateLog(string message)
        {
            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke(new Action(() => UpdateLog(message)));
            }
            else
            {
                txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
            }
        }

        // Hàm cập nhật Thanh tiến trình nhận file (% ProgressBar) an toàn từ luồng phụ
        private void UpdateProgressBar(int percentage)
        {
            if (prgUploadProgress.InvokeRequired)
            {
                prgUploadProgress.Invoke(new Action(() => UpdateProgressBar(percentage)));
            }
            else
            {
                // Giới hạn giá trị trong khoảng 0 - 100
                prgUploadProgress.Value = Math.Max(0, Math.Min(100, percentage));

                // Nếu đạt 100% thì thông báo hoặc đặt lại trạng thái nếu cần
                if (percentage >= 100)
                {
                    lblStatusDetails.Text = "Trạng thái: Đã nhận file thành công!";
                }
                else
                {
                    lblStatusDetails.Text = $"Đang tiến hành tải: {percentage}%";
                }
            }
        }

        // Khi tắt Form, đảm bảo giải phóng và đóng tất cả các kết nối Socket đang chạy ngầm
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _server?.Stop();
        }

        private void lstClients_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnStartServer_Click_1(object sender, EventArgs e)
        {

        }

        private void prgUploadProgress_Click(object sender, EventArgs e)
        {

        }

        private void txtMessageInput_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}