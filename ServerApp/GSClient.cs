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
        private Form _mainMenu;

        public GSClient(Form mainMenu)
        {
            InitializeComponent();
            this._mainMenu = mainMenu;
        }

        private void GSClient_Load(object sender, EventArgs e)
        {
            SocketServer.Instance.OnClientListChanged += UpdateClientListUI;
            UpdateClientListUI();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void UpdateClientListUI()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(UpdateClientListUI));
                return;
            }

            lstClients.Items.Clear();

            lock (SocketServer.Instance.ConnectedClients)
            {
                foreach (var client in SocketServer.Instance.ConnectedClients)
                {
                    if (client.Socket != null && client.Socket.Connected)
                    {
                        // ĐÃ SỬA: Đưa nguyên đối tượng client vào đây (ListBox tự gọi ToString() để hiển thị)
                        lstClients.Items.Add(client);
                    }
                }
            }
            lblTotalClients.Text = $"Tổng số máy đang kết nối: {SocketServer.Instance.ConnectedClients.Count}";
            
        }

        private void lstClients_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Thao tác này giờ hoạt động hoàn hảo vì SelectedItem đang giữ thực thể ClientInfo thực thụ
            if (lstClients.SelectedItem is ClientInfo selectedClient)
            {
                try
                {
                    var ipEnd = (System.Net.IPEndPoint)selectedClient.Socket.Client.RemoteEndPoint;

                    // Đổ chính xác thông tin vào các Label/TextBox bên cột phải của bạn
                    lblIP.Text = $"Địa chỉ IP: {ipEnd.Address}";
                    lblPort.Text = $"Cổng kết nối: {ipEnd.Port}";
                    lblStatus.Text = $"Trạng thái: Đang hoạt động (Online)";

                    // Nếu trong ClientInfo của bạn có lưu biến thời gian đăng nhập (ví dụ LoginTime)
                    lblTime.Text = $"Thời gian vào: {selectedClient.LoginTime:HH:mm:ss}";
                }
                catch
                {
                    ClearDetails();
                }
            }
            else
            {
                ClearDetails();
            }
        }

        // Hàm dọn chữ khi không click chọn ai
        private void ClearDetails()
        {
            lblIP.Text = "Địa chỉ IP:";
            lblPort.Text = "Cổng kết nối:";
            lblStatus.Text = "Trạng thái:";
        }

        private void btnBackToMenu_Click(object sender, EventArgs e)
        {
            if (_mainMenu != null)
            {
                _mainMenu.Show();
            }
            this.Close();
        }

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

        // --- NÚT ĐÁ MÁY CLIENT (KICK) ---
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
                    // Tìm đối tượng cần đá dựa trên chuỗi định dạng hiển thị
                    var clientToKick = SocketServer.Instance.ConnectedClients.FirstOrDefault(c =>
                        $"{c.Name} ({(IPEndPoint)c.Socket.Client.RemoteEndPoint})" == selectedText);

                    if (clientToKick != null)
                    {
                        clientToKick.Socket.Close(); // Ngắt kết nối socket của client đó
                        MessageBox.Show("Đã ngắt kết nối thành công!");
                    }
                }
            }
        }

        // --- NÚT GỬI CẢNH BÁO THẬT SỰ ĐÃ SỬA CHUẨN ĐƯỜNG TRUYỀN ---
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
                // Tìm chính xác đối tượng nhận cảnh báo
                var targetClient = SocketServer.Instance.ConnectedClients.FirstOrDefault(c =>
                    $"{c.Name} ({(IPEndPoint)c.Socket.Client.RemoteEndPoint})" == selectedText);

                if (targetClient != null && targetClient.Socket.Connected)
                {
                    try
                    {
                        NetworkStream stream = targetClient.Socket.GetStream();

                        // Định dạng chuỗi payload gửi đi bao gồm tiền tố phân biệt BUZZ|
                        byte[] data = Encoding.UTF8.GetBytes("BUZZ|Hành vi của bạn đang bị giám sát!");
                        stream.Write(data, 0, data.Length);
                        stream.Flush(); // Ép luồng mạng đẩy gói tin đi ngay lập tức không đợi bộ đệm

                        MessageBox.Show($"Đã gửi lệnh cảnh báo đến máy {targetClient.Name}!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi đường truyền thiết bị: {ex.Message}");
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy thực thể kết nối của Client này, có thể máy đã Offline.");
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