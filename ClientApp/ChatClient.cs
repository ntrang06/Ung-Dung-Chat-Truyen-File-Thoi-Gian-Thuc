using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.IO;
using System.Threading; // <-- BẮT BUỘC thêm thư viện này để quản lý luồng Thread

namespace ClientApp
{
    public partial class lstChat : Form
    {
        private TcpClient client;
        private NetworkStream stream;
        private Thread receiveThread;
        private MainClient _mainMenu;

        // 1. QUAN TRỌNG: Giữ hàm khởi tạo KHÔNG THAM SỐ để đúng với thiết kế và gọi từ MainClient
        public lstChat(MainClient mainClient)
        {
            InitializeComponent();
            this._mainMenu = mainClient;
        }
        public lstChat()
        {
            InitializeComponent();
        }

        // 2. Lấy kết nối từ SocketClient.Instance khi Form bắt đầu tải lên
        private void lstChat_Load(object sender, EventArgs e)
        {
            SocketClient.Instance.OnChatReceived += Socket_OnChatReceived;
            SocketClient.Instance.OnFileReceived += Socket_OnFileReceived;
            try
            {
                // Lấy trực tiếp kết nối TCP thực tế từ SocketClient thông qua thuộc tính đã sửa ở Bước 1
                this.client = SocketClient.Instance.Client;



                if (this.client != null && this.client.Connected)
                {
                    this.stream = this.client.GetStream();
                }
                else
                {
                    txtChatHistory.AppendText("⚠️ Lỗi: Chưa kết nối đến Server!" + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể lấy luồng mạng: " + ex.Message);
            }
        }
        private void Socket_OnChatReceived(string msg)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => Socket_OnChatReceived(msg)));
                return;
            }

            txtChatHistory.AppendText(msg + Environment.NewLine + Environment.NewLine);
        }

        private void Socket_OnFileReceived(string file)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => Socket_OnFileReceived(file)));
                return;
            }

            txtChatHistory.AppendText("📥 Đã nhận file: " + file + Environment.NewLine);
        }
        private void btnBackToMenu_Click(object sender, EventArgs e)
        {
            if (_mainMenu != null)
            {
                _mainMenu.Show();
            }
            this.Close();
        }

        // 2. HÀM GỬI TIN NHẮN (Giữ nguyên cấu trúc cờ hiệu 0x01 của bạn)
        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (stream == null)
                {
                    txtChatHistory.AppendText("⚠️ Lỗi: Chưa lấy được luồng mạng Stream!" + Environment.NewLine);
                    return;
                }

                string message = txtMessage.Text.Trim();
                
                if (string.IsNullOrEmpty(message))
                    return;

                // Đóng gói đúng giao thức
                byte[] msgBytes = Encoding.UTF8.GetBytes(message);
                byte[] lenBytes = BitConverter.GetBytes(msgBytes.Length);

                stream.WriteByte(0x01);
                stream.Write(lenBytes, 0, 4);
                stream.Write(msgBytes, 0, msgBytes.Length);
                stream.Flush();

                txtMessage.Clear();
                txtMessage.Focus();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi gửi tin nhắn từ Client: " + ex.Message);
            }
        }

        // 3. HÀM GỬI FILE (Giữ nguyên cấu trúc cờ hiệu 0x02 của bạn)
        private void btnSendFile_Click(object sender, EventArgs e)
        {
            if (client == null || !client.Connected || stream == null)
            {
                MessageBox.Show("Vui lòng kết nối đến Server trước!");
                return;
            }

            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() != DialogResult.OK) return;

            try
            {
                string filePath = ofd.FileName;
                string fileName = Path.GetFileName(filePath);
                byte[] fileData = File.ReadAllBytes(filePath);

                stream.WriteByte(0x02); // Cờ hiệu nhận diện FILE

                // Độ dài tên file
                byte[] nameBytes = Encoding.UTF8.GetBytes(fileName);
                stream.Write(BitConverter.GetBytes(nameBytes.Length), 0, 4);

                // Tên file
                stream.Write(nameBytes, 0, nameBytes.Length);

                // Kích thước file (8 byte)
                long fileSize = fileData.LongLength;
                stream.Write(BitConverter.GetBytes(fileSize), 0, 8);

                // Nội dung file
                stream.Write(fileData, 0, fileData.Length);
                stream.Flush();

                txtChatHistory.AppendText("📤 Đã gửi file: " + fileName + Environment.NewLine);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi gửi file: " + ex.Message);
            }
        }
        // Đảm bảo đóng kết nối an toàn khi người dùng tắt Form chat
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Nếu muốn quay lại menu mà giữ kết nối, chỉ cần không đóng client ở đây.
            // Ngược lại, nếu muốn ngắt kết nối luôn thì kích hoạt 2 dòng dưới:
            // stream?.Close();
            // client?.Close();
            base.OnFormClosing(e);
        }
        private void label1_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void txtMessage_TextChanged(object sender, EventArgs e) { }
    }
}