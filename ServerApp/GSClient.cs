using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
            SocketServer.Instance.OnClientListChanged += UpdateClientListUI;
            UpdateClientListUI();
            // Thiết lập vị trí Form hiện ra ngay chính giữa màn hình máy tính
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        private void UpdateClientListUI()
        {
            if (lstClients.InvokeRequired)
            {
                lstClients.Invoke(new Action(UpdateClientListUI));
                return;
            }

            lstClients.Items.Clear();

            lock (SocketServer.Instance.ConnectedClients)
            {
                foreach (var client in SocketServer.Instance.ConnectedClients)
                {
                    if (client.Connected)
                    {
                        IPEndPoint ipEnd = (IPEndPoint)client.Client.RemoteEndPoint;
                        lstClients.Items.Add($"{ipEnd.Address}:{ipEnd.Port}");
                    }
                }
                // Cập nhật nhãn tổng số lượng máy ở bên dưới
                lblTotalClients.Text = $"Tổng số máy đang kết nối: {SocketServer.Instance.ConnectedClients.Count}";
            }
        }
        private void lstClients_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstClients.SelectedItem == null) return;

            string selectedText = lstClients.SelectedItem.ToString();
            string[] parts = selectedText.Split(':');

            if (parts.Length == 2)
            {
                lblIP.Text = $"Địa chỉ IP: {parts[0]}";
                lblPort.Text = $"Cổng kết nối: {parts[1]}";
                lblTime.Text = $"Thời gian vào: {DateTime.Now.ToString("HH:mm:ss")}";
                lblStatus.Text = "Trạng thái: Đang kết nối";
            }
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
            SocketServer.Instance.OnClientListChanged -= UpdateClientListUI;
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
            if (lstClients.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn một máy Client để ngắt kết nối!", "Thông báo");
                return;
            }

            string selectedText = lstClients.SelectedItem.ToString();
            DialogResult result = MessageBox.Show($"Ngắt kết nối {selectedText}?", "Xác nhận", MessageBoxButtons.OKCancel);

            if (result == DialogResult.OK)
            {
                lock (SocketServer.Instance.ConnectedClients)
                {
                    var clientToKick = SocketServer.Instance.ConnectedClients.FirstOrDefault(c =>
                        c.Connected && ((IPEndPoint)c.Client.RemoteEndPoint).ToString() == selectedText);

                    if (clientToKick != null)
                    {
                        clientToKick.Close(); // Đóng kết nối ngầm
                        MessageBox.Show("Đã ngắt kết nối thành công!");
                    }
                }
            }
        }

        private void btnBuzz_Click(object sender, EventArgs e)
        {
            if (lstClients.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn một máy Client để gửi cảnh báo!");
                return;
            }

            string selectedText = lstClients.SelectedItem.ToString();

            lock (SocketServer.Instance.ConnectedClients)
            {
                var targetClient = SocketServer.Instance.ConnectedClients.FirstOrDefault(c =>
                    c.Connected && ((IPEndPoint)c.Client.RemoteEndPoint).ToString() == selectedText);

                if (targetClient != null)
                {
                    try
                    {
                        NetworkStream stream = targetClient.GetStream();
                        byte[] data = Encoding.UTF8.GetBytes("BUZZ|Hành vi của bạn đang bị giám sát!");
                        stream.Write(data, 0, data.Length);
                        MessageBox.Show($"Đã gửi lệnh cảnh báo đến máy {selectedText}!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi: {ex.Message}");
                    }
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            UpdateClientListUI();
            MessageBox.Show("Đã cập nhật danh sách Client mới nhất!");
        }
    }
}