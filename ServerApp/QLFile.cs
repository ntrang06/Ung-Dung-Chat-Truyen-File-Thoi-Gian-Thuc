using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace ServerApp
{
    public partial class QLFile : Form
    {
        private Form _mainMenu;

        // Đường dẫn thư mục lưu trữ mặc định trên máy Server (khi tải file từ Client về)
        private string _serverDownloadPath = Path.Combine(Application.StartupPath, "ServerFiles");

        // Hàm khởi tạo nhận MainForm truyền vào từ Menu chính
        public QLFile(Form mainMenu)
        {
            InitializeComponent();
            this._mainMenu = mainMenu;
        }

        private void QLFile_Load(object sender, EventArgs e)
        {
            // Đặt vị trí Form hiển thị ngay giữa màn hình
            this.StartPosition = FormStartPosition.CenterScreen;

            txtCurrentPath.Text = _serverDownloadPath;

            lvFiles.View = View.Details;
            lvFiles.FullRowSelect = true;
            if (!Directory.Exists(_serverDownloadPath))
            {
                Directory.CreateDirectory(_serverDownloadPath);
            }
            LoadServerFiles();
        }
        private void RequestClientFiles(string targetPath)
        {
            // Cấu trúc gói lệnh gửi đi: REQ_FILES|Đường_dẫn
            byte[] requestData = Encoding.UTF8.GetBytes("REQ_FILES|" + targetPath);

            lock (SocketServer.Instance.ConnectedClients)
            {
                foreach (var client in SocketServer.Instance.ConnectedClients)
                {
                    if (client.Socket != null && client.Socket.Connected)
                    {
                        try
                        {
                            NetworkStream stream = client.Socket.GetStream();
                            stream.Write(requestData, 0, requestData.Length);
                            break; // Gửi lệnh tới máy Client đầu tiên đang online để tương tác quản lý
                        }
                        catch { }
                    }
                }
            }
        }
        private void PopulateListView(string fileData)
        {
            MessageBox.Show(fileData);
            if (lvFiles.InvokeRequired)
            {
                lvFiles.Invoke(new Action<string>(PopulateListView), fileData);
                return;
            }

            lvFiles.Items.Clear(); // Xóa sạch các dòng cũ trên giao diện UI

            // Chuỗi dữ liệu từ Client trả về phân tách bằng dấu | : Tên*KíchThước*ĐịnhDạng|Tên2*KíchThước2*ĐịnhDạng2
            string[] files = fileData.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string fileInfo in files)
            {
                string[] details = fileInfo.Split('*');
                if (details.Length == 3)
                {
                    ListViewItem item = new ListViewItem(details[0]); // Cột 1: Tên File / Thư mục
                    item.SubItems.Add(details[1]);                    // Cột 2: Kích thước
                    item.SubItems.Add(details[2]);                    // Cột 3: Định dạng

                    lvFiles.Items.Add(item); // Thêm dòng vào ListView
                }
            }
        }

        // --- CẢI TIẾN 2: BẤM NÚT [...] ĐỂ CHỌN THƯ MỤC LƯU TRÊN SERVER ---
        private void btnChooseSaveLocation_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Chọn thư mục trên máy Server để lưu các file tải từ Client về:";

                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    // Cập nhật vị trí lưu mới được chọn
                    _serverDownloadPath = fbd.SelectedPath;
                    txtCurrentPath.Text = _serverDownloadPath;

                    LoadServerFiles();

                    MessageBox.Show("Đã thay đổi nơi lưu.");

                    MessageBox.Show($"Đã thay đổi nơi lưu trữ tải về thành công!\nThư mục hiện tại: {_serverDownloadPath}",
                                    "Cấu hình Server",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                }
            }
        }

        // --- CẢI TIẾN 1: TỰ GÕ ĐƯỜNG DẪN TRÊN TEXTBOX RỒI NHẤN ENTER ---
        private void txtCurrentPath_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string customPath = txtCurrentPath.Text.Trim();
                if (!string.IsNullOrEmpty(customPath))
                {
                    RequestClientFiles(customPath);
                }
            }
        }

        // --- SỰ KIỆN: CLICK ĐÚP VÀO THƯ MỤC TRÊN LISTVIEW ĐỂ MỞ THƯ MỤC CON ---
        private void lvFiles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lvFiles.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = lvFiles.SelectedItems[0];

                // Kiểm tra xem cột Định dạng (Cột thứ 3 - Index số 2) có phải là "Thư mục" hay không
                if (selectedItem.SubItems[2].Text == "Thư mục")
                {
                    string folderName = selectedItem.Text;
                    string currentPath = txtCurrentPath.Text;

                    // Đảm bảo xử lý chuẩn dấu xuyệt đường dẫn hệ thống
                    if (!currentPath.EndsWith(@"\"))
                    {
                        currentPath += @"\";
                    }

                    string newPath = currentPath + folderName;
                    txtCurrentPath.Text = newPath; // Cập nhật đường dẫn mới lên thanh tìm kiếm

                    // Phát lệnh qua mạng yêu cầu Client nạp lại dữ liệu thư mục con này
                    RequestClientFiles(newPath);
                }
            }
        }

        // --- NÚT TẢI FILE VỀ MÁY ---
        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (lvFiles.SelectedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn file.");
                return;
            }

            string fileName = lvFiles.SelectedItems[0].Text;


            string sourceFile = Path.Combine(_serverDownloadPath, fileName);

            if (!File.Exists(sourceFile))
            {
                MessageBox.Show("Không tìm thấy file trên Server.");
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = fileName;
            sfd.Filter = "Tất cả file|*.*";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                File.Copy(sourceFile, sfd.FileName, true);

                MessageBox.Show("Tải file hoàn tất!");
            }
        }

        // --- NÚT XÓA FILE ---
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lvFiles.SelectedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn file.");
                return;
            }

            string fileName = lvFiles.SelectedItems[0].Text;

            string filePath = Path.Combine(_serverDownloadPath, fileName);

            if (!File.Exists(filePath))
            {
                MessageBox.Show("Không tìm thấy file.");
                return;
            }

            File.Delete(filePath);

            LoadServerFiles();

            MessageBox.Show("Đã xóa file.");
        }
        private void LoadServerFiles()
        {
            lvFiles.Items.Clear();

            DirectoryInfo dir = new DirectoryInfo(_serverDownloadPath);

            foreach (FileInfo file in dir.GetFiles())
            {
                ListViewItem item = new ListViewItem(file.Name);

                item.SubItems.Add((file.Length / 1024) + " KB");

                item.SubItems.Add(file.Extension);

                lvFiles.Items.Add(item);
            }
        }

        // --- NÚT LÀM MỚI DANH SÁCH FILE ---
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadServerFiles();
        }

        // --- NÚT QUAY LẠI MENU CHÍNH ---
        private void btnBackToMenu_Click(object sender, EventArgs e)
        {
            if (_mainMenu != null)
            {
                _mainMenu.Show();
            }
            this.Close();
        }

        // --- HÀM BẢO VỆ KHI BẤM DẤU [X] ĐỎ GÓC FORM ---
        private void QLFile_FormClosing(object sender, FormClosingEventArgs e)
        {
            SocketServer.Instance.OnFilesReceived -= PopulateListView;
            if (_mainMenu != null && !_mainMenu.Visible)
            {
                _mainMenu.Show();
            }
        }

        private void QLFile_Load_1(object sender, EventArgs e)
        {

        }
    }
}
