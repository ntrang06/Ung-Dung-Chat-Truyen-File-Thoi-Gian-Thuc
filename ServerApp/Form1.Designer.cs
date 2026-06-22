namespace ServerApp
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnDungServer = new System.Windows.Forms.Button();
            this.btnKhoiDongServer = new System.Windows.Forms.Button();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.lstClients = new System.Windows.Forms.ListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnSendMessage = new System.Windows.Forms.Button();
            this.txtMessageInput = new System.Windows.Forms.TextBox();
            this.txtChatHistory = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lblStatusDetails = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dgvFiles = new System.Windows.Forms.DataGridView();
            this.colTenFile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colKichThuoc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colThoiGianNhan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTrangThai = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.prgUploadProgress = new System.Windows.Forms.ProgressBar();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFiles)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnDungServer);
            this.groupBox1.Controls.Add(this.btnKhoiDongServer);
            this.groupBox1.Controls.Add(this.txtPort);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(826, 341);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Cấu hình Hệ thống";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(45, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Cổng (Port):";
            // 
            // btnDungServer
            // 
            this.btnDungServer.Location = new System.Drawing.Point(293, 143);
            this.btnDungServer.Name = "btnDungServer";
            this.btnDungServer.Size = new System.Drawing.Size(109, 41);
            this.btnDungServer.TabIndex = 2;
            this.btnDungServer.Text = "Dừng";
            this.btnDungServer.UseVisualStyleBackColor = true;
            // 
            // btnKhoiDongServer
            // 
            this.btnKhoiDongServer.Location = new System.Drawing.Point(76, 138);
            this.btnKhoiDongServer.Name = "btnKhoiDongServer";
            this.btnKhoiDongServer.Size = new System.Drawing.Size(108, 46);
            this.btnKhoiDongServer.TabIndex = 1;
            this.btnKhoiDongServer.Text = "Khởi động";
            this.btnKhoiDongServer.UseVisualStyleBackColor = true;
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(166, 47);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(100, 26);
            this.txtPort.TabIndex = 0;
            this.txtPort.Text = "8080";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtLog);
            this.groupBox2.Controls.Add(this.lstClients);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 350);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(826, 452);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Giám sát Client";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(388, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(142, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "Nhật ký hoạt động:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(72, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Danh sách Client";
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(553, 56);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(208, 38);
            this.txtLog.TabIndex = 4;
            // 
            // lstClients
            // 
            this.lstClients.FormattingEnabled = true;
            this.lstClients.ItemHeight = 20;
            this.lstClients.Location = new System.Drawing.Point(21, 65);
            this.lstClients.Name = "lstClients";
            this.lstClients.Size = new System.Drawing.Size(232, 264);
            this.lstClients.TabIndex = 3;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnSendMessage);
            this.groupBox3.Controls.Add(this.txtMessageInput);
            this.groupBox3.Controls.Add(this.txtChatHistory);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(835, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(928, 341);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Khung Chat Hệ thống";
            // 
            // btnSendMessage
            // 
            this.btnSendMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnSendMessage.Location = new System.Drawing.Point(3, 288);
            this.btnSendMessage.Name = "btnSendMessage";
            this.btnSendMessage.Size = new System.Drawing.Size(922, 50);
            this.btnSendMessage.TabIndex = 7;
            this.btnSendMessage.Text = "Gửi tin nhắn";
            this.btnSendMessage.UseVisualStyleBackColor = true;
            // 
            // txtMessageInput
            // 
            this.txtMessageInput.Location = new System.Drawing.Point(3, 81);
            this.txtMessageInput.Name = "txtMessageInput";
            this.txtMessageInput.Size = new System.Drawing.Size(922, 26);
            this.txtMessageInput.TabIndex = 6;
            // 
            // txtChatHistory
            // 
            this.txtChatHistory.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtChatHistory.Location = new System.Drawing.Point(3, 22);
            this.txtChatHistory.Multiline = true;
            this.txtChatHistory.Name = "txtChatHistory";
            this.txtChatHistory.ReadOnly = true;
            this.txtChatHistory.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtChatHistory.Size = new System.Drawing.Size(922, 26);
            this.txtChatHistory.TabIndex = 5;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lblStatusDetails);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.dgvFiles);
            this.groupBox4.Controls.Add(this.prgUploadProgress);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(835, 350);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(928, 452);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Quản lý File";
            // 
            // lblStatusDetails
            // 
            this.lblStatusDetails.AutoSize = true;
            this.lblStatusDetails.Location = new System.Drawing.Point(362, 298);
            this.lblStatusDetails.Name = "lblStatusDetails";
            this.lblStatusDetails.Size = new System.Drawing.Size(230, 20);
            this.lblStatusDetails.TabIndex = 11;
            this.lblStatusDetails.Text = "Tốc độ: 0 KB/s | Đã nhận: 0 MB";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(172, 264);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(110, 20);
            this.label4.TabIndex = 10;
            this.label4.Text = "Tiến độ tải file:";
            // 
            // dgvFiles
            // 
            this.dgvFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFiles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTenFile,
            this.colKichThuoc,
            this.colThoiGianNhan,
            this.colTrangThai});
            this.dgvFiles.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvFiles.Location = new System.Drawing.Point(3, 22);
            this.dgvFiles.Name = "dgvFiles";
            this.dgvFiles.RowHeadersWidth = 62;
            this.dgvFiles.Size = new System.Drawing.Size(922, 195);
            this.dgvFiles.TabIndex = 9;
            // 
            // colTenFile
            // 
            this.colTenFile.HeaderText = "Tên File";
            this.colTenFile.MinimumWidth = 8;
            this.colTenFile.Name = "colTenFile";
            this.colTenFile.Width = 150;
            // 
            // colKichThuoc
            // 
            this.colKichThuoc.HeaderText = "Kích Thước";
            this.colKichThuoc.MinimumWidth = 8;
            this.colKichThuoc.Name = "colKichThuoc";
            this.colKichThuoc.Width = 150;
            // 
            // colThoiGianNhan
            // 
            this.colThoiGianNhan.HeaderText = "Thời Gian Nhận ";
            this.colThoiGianNhan.MinimumWidth = 8;
            this.colThoiGianNhan.Name = "colThoiGianNhan";
            this.colThoiGianNhan.Width = 150;
            // 
            // colTrangThai
            // 
            this.colTrangThai.HeaderText = "Trạng Thái ";
            this.colTrangThai.MinimumWidth = 8;
            this.colTrangThai.Name = "colTrangThai";
            this.colTrangThai.Width = 150;
            // 
            // prgUploadProgress
            // 
            this.prgUploadProgress.Location = new System.Drawing.Point(319, 261);
            this.prgUploadProgress.Name = "prgUploadProgress";
            this.prgUploadProgress.Size = new System.Drawing.Size(315, 23);
            this.prgUploadProgress.TabIndex = 8;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 47.14785F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 52.85215F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox4, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox3, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 43.22981F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 56.77019F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1766, 805);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1766, 805);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFiles)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnDungServer;
        private System.Windows.Forms.Button btnKhoiDongServer;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.ListBox lstClients;
        private System.Windows.Forms.Button btnSendMessage;
        private System.Windows.Forms.TextBox txtMessageInput;
        private System.Windows.Forms.TextBox txtChatHistory;
        private System.Windows.Forms.DataGridView dgvFiles;
        private System.Windows.Forms.ProgressBar prgUploadProgress;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblStatusDetails;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenFile;
        private System.Windows.Forms.DataGridViewTextBoxColumn colKichThuoc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colThoiGianNhan;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTrangThai;
    }
}

