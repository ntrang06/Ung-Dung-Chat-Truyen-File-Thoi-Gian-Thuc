using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientApp
{
    public partial class ClientFileForm : Form
    {
        public ClientFileForm()
        {
            InitializeComponent();
        }
        private MainClient _mainMenu;
        private void ClientFileForm_Load(object sender, EventArgs e)
        {
            SocketClient.Instance.OnCommandReceived += HandleCommand;

            txtPath.ReadOnly = true;

            lvFile.Columns.Clear();
            lvFile.Columns.Add("FileName", "Tên File");
            lvFile.Columns.Add("Size", "Kích thước");
            lvFile.Columns.Add("Type", "Định dạng");

            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
            progressBar1.Value = 0;
        }
        private void HandleFileReceived(string fileName)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(HandleFileReceived), fileName);
                return;
            }

            MessageBox.Show("Đã nhận file:\n" + fileName);

            progressBar1.Value = 100;
        }
       
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            lvFile.Items.Clear();

            SocketClient.Instance.SendData("REQ_FILES|" + txtPath.Text);
        }
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text = openFileDialog1.FileName;
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (txtPath.Text == "")
            {
                MessageBox.Show("Vui lòng chọn file.");
                return;
            }

            try
            {
                NetworkStream stream = SocketClient.Instance.GetStream();

                if (stream == null)
                {
                    MessageBox.Show("Chưa kết nối Server.");
                    return;
                }

                string fileName = Path.GetFileName(txtPath.Text);

                byte[] fileData = File.ReadAllBytes(txtPath.Text);

                progressBar1.Value = 0;

                stream.WriteByte(0x02);

                byte[] nameBytes = Encoding.UTF8.GetBytes(fileName);

                stream.Write(BitConverter.GetBytes(nameBytes.Length), 0, 4);
                stream.Write(nameBytes, 0, nameBytes.Length);

                long fileSize = fileData.LongLength;

                stream.Write(BitConverter.GetBytes(fileSize), 0, 8);

                stream.Write(fileData, 0, fileData.Length);

                stream.Flush();

                progressBar1.Value = 100;

                lvFile.Items.Add(new ListViewItem(new string[]
                {
                    fileName,
                    (fileSize / 1024) + " KB",
                    Path.GetExtension(fileName)
                }));

                MessageBox.Show("Upload thành công.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void HandleCommand(string cmd)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(HandleCommand), cmd);
                return;
            }

            if (cmd.StartsWith("DELETE_FILE|"))
            {
                string path = cmd.Substring("DELETE_FILE|".Length).Trim();

                try
                {
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                        MessageBox.Show("Đã xóa file:\n" + path);
                    }
                    else if (Directory.Exists(path))
                    {
                        Directory.Delete(path, true);
                        MessageBox.Show("Đã xóa thư mục:\n" + path);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            else if (cmd.StartsWith("REQ_FILES|"))
            {
                string path = cmd.Substring("REQ_FILES|".Length).Trim();

                if (!Directory.Exists(path))
                    return;

                StringBuilder sb = new StringBuilder();

                DirectoryInfo dir = new DirectoryInfo(path);

                foreach (DirectoryInfo d in dir.GetDirectories())
                {
                    sb.Append($"{d.Name}*<DIR>*Thư mục|");
                }

                foreach (FileInfo f in dir.GetFiles())
                {
                    sb.Append($"{f.Name}*{f.Length / 1024} KB*{f.Extension}|");
                }

                SocketClient.Instance.SendData(sb.ToString());
            }

            else if (cmd.StartsWith("DOWNLOAD|"))
            {
                string path = cmd.Substring("DOWNLOAD|".Length).Trim();

                if (!File.Exists(path))
                    return;

                NetworkStream stream = SocketClient.Instance.GetStream();

                byte[] file = File.ReadAllBytes(path);

                stream.WriteByte(0x02);

                byte[] name = Encoding.UTF8.GetBytes(Path.GetFileName(path));

                stream.Write(BitConverter.GetBytes(name.Length), 0, 4);
                stream.Write(name, 0, name.Length);

                stream.Write(BitConverter.GetBytes((long)file.Length), 0, 8);

                stream.Write(file, 0, file.Length);

                stream.Flush();
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
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }
    }
}
