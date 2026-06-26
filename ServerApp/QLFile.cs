using System;
using System.IO;
using System.Windows.Forms;

namespace ServerApp
{
    public partial class QLFile : Form
    {
        private Form _mainMenu;

        // Đường dẫn thư mục lưu trữ mặc định trên máy Server (khi tải file từ Client về)
        private string _serverDownloadPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

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

            // Đặt đường dẫn ổ đĩa mặc định hiển thị trên thanh tìm kiếm
            txtCurrentPath.Text = @"C:\";

            // Cấu hình ListView chọn nguyên dòng trực quan
            lvFiles.View = View.Details;
            lvFiles.FullRowSelect = true;
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
                    // TODO: Gửi chuỗi gói tin yêu cầu quét thư mục (ví dụ: "REQ_DIR|" + customPath) sang Client qua Stream
                    MessageBox.Show($"Đang gửi lệnh yêu cầu Client mở thư mục tự chọn:\n{customPath}", "Gửi yêu cầu");
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

                    // Đảm bảo dấu xuyệt đường dẫn chính xác
                    if (!currentPath.EndsWith(@"\"))
                    {
                        currentPath += @"\";
                    }

                    // Cập nhật đường dẫn mới lên thanh TextBox
                    txtCurrentPath.Text = currentPath + folderName;

                    // TODO: Gửi lệnh Socket yêu cầu Client nạp lại danh sách file trong thư mục mới này
                    MessageBox.Show($"Yêu cầu Client mở thư mục con: {txtCurrentPath.Text}", "Điều hướng nhanh");
                }
            }
        }

        // --- NÚT TẢI FILE VỀ MÁY ---
        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (lvFiles.SelectedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một file trong danh sách để tải về máy!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ListViewItem selectedItem = lvFiles.SelectedItems[0];
            if (selectedItem.SubItems[2].Text == "Thư mục")
            {
                MessageBox.Show("Hệ thống chỉ hỗ trợ tải file đơn lẻ, không hỗ trợ tải nguyên thư mục!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string fileName = selectedItem.Text;
            // TODO: Triển khai luồng đọc mảng Byte truyền qua mạng và ghi file xuống '_serverDownloadPath'
            MessageBox.Show($"Đang kết nối nhận luồng dữ liệu file [{fileName}] từ máy con...\nNơi lưu trữ: {_serverDownloadPath}", "Đang tiến hành tải");
        }

        // --- NÚT XÓA FILE ---
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lvFiles.SelectedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một file hoặc thư mục để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string targetName = lvFiles.SelectedItems[0].Text;

            // Hiện hộp thoại xác nhận trước khi xóa tránh nhầm lẫn
            DialogResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa vĩnh viễn [{targetName}] trên máy Client không?",
                                                  "Xác nhận xóa",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                // TODO: Gửi gói lệnh yêu cầu xóa file/thư mục này trên máy Client
                MessageBox.Show($"Đã gửi lệnh xóa file [{targetName}] tới máy con.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // --- NÚT LÀM MỚI DANH SÁCH FILE ---
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            // TODO: Gửi lệnh yêu cầu Client quét lại thư mục hiện tại để đồng bộ danh sách mới nhất
            MessageBox.Show("Đang cập nhật làm mới danh sách thư mục từ Client...", "Đồng bộ");
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