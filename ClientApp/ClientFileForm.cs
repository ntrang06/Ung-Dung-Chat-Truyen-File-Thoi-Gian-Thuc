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

        private void ClientFileForm_Load(object sender, EventArgs e)
        {
            txtPath.ReadOnly = true;

            lvFile.Columns.Clear();
            lvFile.Columns.Add("FileName", "Tên File");
            lvFile.Columns.Add("Size", "Kích thước");
            lvFile.Columns.Add("Type", "Định dạng");

            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
            progressBar1.Value = 0;
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

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }
    }
}
