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

            // Đăng ký nhận sự kiện từ lõi Socket gửi ra ngoài giao diện UI
            _server.OnLogReceived += UpdateLog;
            _server.OnClientConnected += AddClientToUI;
            _server.OnClientDisconnected += RemoveClientFromUI;
            _server.OnMessageReceived += UpdateChatHistory;
            _server.OnProgressUpdated += UpdateProgressBar;
            _server.OnFileReceived += AddFileToGrid;
        }

        // Sự kiện bấm nút Khởi động Server
        private void btnKhoiDongServer_Click(object sender, EventArgs e)
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
                MessageBox.Show("Vui lòng nhập số Cổng (Port) hợp lệ!", "Lỗi cấu hình", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Sự kiện bấm nút Dừng Server
        private void btnDungServer_Click(object sender, EventArgs e)
        {
            _server.Stop();
            UpdateLog("Hệ thống Server đã dừng hoạt động hoàn toàn.");
            btnKhoiDongServer.Enabled = true;
            btnDungServer.Enabled = false;
            txtPort.Enabled = true;
        }

        // Sự kiện bấm nút Gửi tin nhắn từ Server cho Client
        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            string msg = txtMessageInput.Text.Trim();
            if (!string.IsNullOrEmpty(msg))
            {
                // Gửi tin nhắn dạng quảng bá (Broadcast) cho tất cả các Client đang kết nối
                _server.BroadcastMessage(msg);
                UpdateChatHistory($"[Server]: {msg}");
                txtMessageInput.Clear();
            }
        }

        // --- CÁC HÀM CẬP NHẬT GIAO DIỆN ĐẢM BẢO AN TOÀN ĐA LUỒNG (THREAD-SAFE UI) ---

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

        private void AddClientToUI(string clientEndPoint)
        {
            if (lstClients.InvokeRequired)
            {
                lstClients.Invoke(new Action(() => AddClientToUI(clientEndPoint)));
            }
            else
            {
                lstClients.Items.Add(clientEndPoint);
            }
        }

        private void RemoveClientFromUI(string clientEndPoint)
        {
            if (lstClients.InvokeRequired)
            {
                lstClients.Invoke(new Action(() => RemoveClientFromUI(clientEndPoint)));
            }
            else
            {
                lstClients.Items.Remove(clientEndPoint);
            }
        }

        private void UpdateChatHistory(string message)
        {
            if (txtChatHistory.InvokeRequired)
            {
                txtChatHistory.Invoke(new Action(() => UpdateChatHistory(message)));
            }
            else
            {
                txtChatHistory.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
            }
        }

        private void UpdateProgressBar(int percentage)
        {
            if (prgUploadProgress.InvokeRequired)
            {
                prgUploadProgress.Invoke(new Action(() => UpdateProgressBar(percentage)));
            }
            else
            {
                prgUploadProgress.Value = Math.Max(0, Math.Min(100, percentage));
                lblStatusDetails.Text = $"Đang tiến hành tải dữ liệu: {percentage}%";
            }
        }

        private void AddFileToGrid(string fileName, string fileSize, string status)
        {
            if (dgvFiles.InvokeRequired)
            {
                dgvFiles.Invoke(new Action(() => AddFileToGrid(fileName, fileSize, status)));
            }
            else
            {
                string timeString = DateTime.Now.ToString("HH:mm:ss");
                dgvFiles.Rows.Add(fileName, fileSize, timeString, status);
                lblStatusDetails.Text = $"Trạng thái: Đã nhận xong file {fileName}";
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _server?.Stop();
        }
    }
}