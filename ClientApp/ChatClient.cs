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
            try
            {
                // Lấy trực tiếp kết nối TCP thực tế từ SocketClient thông qua thuộc tính đã sửa ở Bước 1
                this.client = SocketClient.Instance.Client;

                if (this.client != null && this.client.Connected)
                {
                    this.stream = this.client.GetStream();

                    receiveThread = new Thread(ReceiveData);
                    receiveThread.IsBackground = true;
                    receiveThread.Start();
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
                if (string.IsNullOrEmpty(message)) return;

                string formattedMessage = "CHAT|" + message;
                byte[] msgBytes = Encoding.UTF8.GetBytes(formattedMessage);

              
                SocketClient.Instance.SendData("CHAT|" + message);

                // 4. Hiển thị lên màn hình chat của chính mình (Client)
                string time = DateTime.Now.ToString("HH:mm:ss");

                //txtChatHistory.AppendText(
                //    $"[{time}] [Bạn]:{Environment.NewLine}{message}{Environment.NewLine}{Environment.NewLine}");

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

        // Hàm đảm bảo đọc đủ số lượng Byte được yêu cầu trên đường truyền mạng
        private void ReadFull(byte[] buffer, int size)
        {
            int offset = 0;
            while (offset < size)
            {
                int read = stream.Read(buffer, offset, size - offset);
                if (read == 0)
                    throw new Exception("Mất kết nối.");
                offset += read;
            }
        }

        // 4. LUỒNG NHẬN DỮ LIỆU TỰ ĐỘNG THỜI GIAN THỰC (Đã sửa lỗi đồng bộ đọc luồng)
        private void ReceiveData()
        {
            try
            {
                while (client != null && client.Connected)
                {
                    int header = stream.ReadByte();
                    if (header == -1) break; // Server ngắt kết nối

                    if (header == 0x01) // TRƯỜNG HỢP 1: Nhận tin nhắn Text từ Server
                    {
                        byte[] lengthBytes = new byte[4];
                        ReadFull(lengthBytes, 4);
                        int msgLen = BitConverter.ToInt32(lengthBytes, 0);

                        byte[] msgBytes = new byte[msgLen];
                        ReadFull(msgBytes, msgLen);
                        string msg = Encoding.UTF8.GetString(msgBytes);
                        
                        Invoke(new Action(() => {
                            txtChatHistory.AppendText(msg + Environment.NewLine + Environment.NewLine);
                        }));
                    }
                    else if (header == 0x02) // TRƯỜNG HỢP 2: Nhận File từ Server gửi qua
                    {
                        // Đọc tên File
                        byte[] nameLenBytes = new byte[4];
                        ReadFull(nameLenBytes, 4);
                        int nameLen = BitConverter.ToInt32(nameLenBytes, 0);

                        byte[] nameBytes = new byte[nameLen];
                        ReadFull(nameBytes, nameLen);
                        string fileName = Encoding.UTF8.GetString(nameBytes);

                        // Đọc kích thước file
                        byte[] sizeBytes = new byte[8];
                        ReadFull(sizeBytes, 8);
                        long fileSize = BitConverter.ToInt64(sizeBytes, 0);

                        // Đọc nội dung dữ liệu file
                        byte[] fileBytes = new byte[fileSize];
                        ReadFull(fileBytes, (int)fileSize);

                        // Lưu file vào thư mục thực thi của ứng dụng
                        File.WriteAllBytes(fileName, fileBytes);

                        Invoke(new Action(() => {;
                            txtChatHistory.AppendText("📥 Đã nhận file: " + fileName + Environment.NewLine);
                        }));
                    }
                }
            }
            catch (Exception)
            {
                if (client != null && client.Connected)
                {
                    Invoke(new Action(() => {
                        txtChatHistory.AppendText("⚠️ Đã xảy ra lỗi khi nhận dữ liệu." + Environment.NewLine);
                    }));
                }
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

        // Các hàm sự kiện click trống có thể giữ lại để tránh lỗi Designer
        private void label1_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void txtMessage_TextChanged(object sender, EventArgs e) { }
    }
}