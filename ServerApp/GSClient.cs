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
            if (lstClients.Items.Count > 0 && lstClients.SelectedIndex == -1)
            {
                lstClients.SelectedIndex = 0; // Tự động bôi xanh máy đầu tiên
            }
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
        // --- NÚT GỬI CẢNH BÁO ĐỒNG LOẠT CHO TẤT CẢ CLIENT ---
        private void btnBuzz_Click(object sender, EventArgs e)
        {
            string buzzMsg = "Hành vi của bạn đang bị giám sát!";

            // Đóng gói mảng byte theo đúng cấu trúc giao thức: [Header] + [Length] + [Content]
            byte[] msgBytes = Encoding.UTF8.GetBytes(buzzMsg);
            byte[] lengthBytes = BitConverter.GetBytes(msgBytes.Length);

            byte[] data = new byte[1 + 4 + msgBytes.Length];
            data[0] = 0x03; 
            Array.Copy(lengthBytes, 0, data, 1, 4);
            Array.Copy(msgBytes, 0, data, 5, msgBytes.Length);

            int count = 0;

            lock (SocketServer.Instance.ConnectedClients)
            {
                foreach (var client in SocketServer.Instance.ConnectedClients)
                {
                    if (client.Socket != null && client.Socket.Connected)
                    {
                        try
                        {
                            NetworkStream stream = client.Socket.GetStream();
                            stream.Write(data, 0, data.Length);
                            stream.Flush();
                            count++;
                        }
                        catch { }
                    }
                }
            }

            if (count > 0)
            {
                MessageBox.Show($"Đã phát lệnh cảnh báo thành công tới toàn bộ {count} máy con!", "Thành công");
            }
            else
            {
                MessageBox.Show("Không có máy Client nào đang kết nối!", "Thông báo");
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            UpdateClientListUI();
            MessageBox.Show("Đã cập nhật danh sách Client mới nhất!");
        }
    }
}