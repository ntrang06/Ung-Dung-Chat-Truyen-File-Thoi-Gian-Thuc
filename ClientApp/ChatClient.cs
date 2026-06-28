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
using System.IO; // <-- Thêm thư viện này để đọc/ghi File

namespace ClientApp
{
    // Giữ nguyên tên lớp là lstChat theo đúng hình chụp hiện tại của bạn
    public partial class lstChat : Form
    {
        private TcpClient client;
        private NetworkStream stream;

        public lstChat()
        {
            InitializeComponent();
        }

        // 1. SỬA LẠI HÀM KẾT NỐI (Thêm luồng tự động nhận tin nhắn/file thời gian thực)
        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                client = new TcpClient();
                client.Connect(txtIP.Text, int.Parse(txtPort.Text));

                stream = client.GetStream();

                MessageBox.Show("Kết nối thành công!");

                // Kích hoạt luồng chạy ngầm để liên tục nghe dữ liệu từ Server mà không bị treo giao diện
                Task.Run(() => ReceiveData());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message);
            }
        }

        // 2. GIỮ NGUYÊN HÀM GỬI TIN NHẮN (Bổ sung hiển thị lên ListBox của bạn)
        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                string message = txtMessage.Text;
                if (string.IsNullOrEmpty(message)) return;

                byte[] msgBytes = Encoding.UTF8.GetBytes(message);
                byte[] lengthBytes = BitConverter.GetBytes(msgBytes.Length);

                stream.WriteByte(0x01); // 0x01: Cờ hiệu nhận diện đây là TIN NHẮN TEXT
                stream.Write(lengthBytes, 0, 4);
                stream.Write(msgBytes, 0, msgBytes.Length);
                stream.Flush();

                // Hiển thị tin nhắn bạn vừa gửi lên listBox1
                listBox1.Items.Add("Bạn: " + message);
                txtMessage.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi gửi tin nhắn: " + ex.Message);
            }
        }

        // 3. THÊM MỚI HÀM GỬI FILE (Liên kết với nút btnSendFile bạn vừa tạo ở Bước 1)
        private void btnSendFile_Click(object sender, EventArgs e)
        {
            if (client == null || !client.Connected)
            {
                MessageBox.Show("Vui lòng kết nối đến Server trước!");
                return;
            }

            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                string filePath = ofd.FileName;
                string fileName = Path.GetFileName(filePath);

                byte[] fileData = File.ReadAllBytes(filePath);

                // Header
                stream.WriteByte(0x02);

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

                listBox1.Items.Add("📤 Đã gửi file: " + fileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
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


        // 4. THÊM MỚI LUỒNG NHẬN DỮ LIỆU TỰ ĐỘNG THỜI GIAN THỰC từ Server đổ về
        private void ReceiveData()
        {
            try
            {
                while (client != null && client.Connected)
                {
                    // Đọc byte đầu tiên (Header) để phân loại gói tin dữ liệu là gì
                    int header = stream.ReadByte();
                    if (header == -1) break; // Server ngắt kết nối đột ngột

                    if (header == 0x01) // TRƯỜNG HỢP 1: Nhận tin nhắn Text từ Server
                    {
                        byte[] lengthBytes = new byte[4];
                        stream.Read(lengthBytes, 0, 4);
                        int msgLen = BitConverter.ToInt32(lengthBytes, 0);

                        byte[] msgBytes = new byte[msgLen];
                        stream.Read(msgBytes, 0, msgLen);
                        string msg = Encoding.UTF8.GetString(msgBytes);

                        // Đồng bộ hiển thị lên ô listBox1 (phải dùng Invoke vì đang ở luồng khác luồng chính)
                        Invoke(new Action(() => {
                            listBox1.Items.Add("Server: " + msg);
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

                        // kích thước file
                        byte[] sizeBytes = new byte[8];
                        ReadFull(sizeBytes, 8);

                        long fileSize = BitConverter.ToInt64(sizeBytes, 0);

                        // dữ liệu file
                        byte[] fileBytes = new byte[fileSize];
                        ReadFull(fileBytes, (int)fileSize);

                        File.WriteAllBytes(fileName, fileBytes);

                        Invoke(new Action(() =>
                        {
                            listBox1.Items.Add("📥 Đã nhận file: " + fileName);
                        }));
                    }
                }
            }
            catch (Exception)
            {
                Invoke(new Action(() => {
                    listBox1.Items.Add("⚠️ Đã mất kết nối tới Server.");
                }));
            }
        }


        private void label1_Click(object sender, EventArgs e)
        {
            // Để trống hoặc xóa đi nếu bạn đã xóa label cũ
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void txtMessage_TextChanged(object sender, EventArgs e)
        {

        }
    }
}