namespace ServerApp
{
    partial class GSClient
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblTotalClients = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnBuzz = new System.Windows.Forms.Button();
            this.btnKick = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblIP = new System.Windows.Forms.Label();
            this.lblPort = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.lstClients = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBackToMenu = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Info;
            this.panel1.Controls.Add(this.lblTotalClients);
            this.panel1.Controls.Add(this.btnRefresh);
            this.panel1.Controls.Add(this.btnBuzz);
            this.panel1.Controls.Add(this.btnKick);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.lstClients);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btnBackToMenu);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(929, 530);
            this.panel1.TabIndex = 0;
            // 
            // lblTotalClients
            // 
            this.lblTotalClients.AutoSize = true;
            this.lblTotalClients.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalClients.Location = new System.Drawing.Point(7, 365);
            this.lblTotalClients.Name = "lblTotalClients";
            this.lblTotalClients.Size = new System.Drawing.Size(285, 26);
            this.lblTotalClients.TabIndex = 14;
            this.lblTotalClients.Text = "Tổng số máy đang kết nối: 0";
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.Tomato;
            this.btnRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnRefresh.Location = new System.Drawing.Point(625, 425);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(238, 50);
            this.btnRefresh.TabIndex = 13;
            this.btnRefresh.Text = "Làm mới danh sách";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnBuzz
            // 
            this.btnBuzz.BackColor = System.Drawing.Color.Tomato;
            this.btnBuzz.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBuzz.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnBuzz.Location = new System.Drawing.Point(354, 425);
            this.btnBuzz.Name = "btnBuzz";
            this.btnBuzz.Size = new System.Drawing.Size(175, 50);
            this.btnBuzz.TabIndex = 12;
            this.btnBuzz.Text = "Gửi cảnh báo";
            this.btnBuzz.UseVisualStyleBackColor = false;
            this.btnBuzz.Click += new System.EventHandler(this.btnBuzz_Click);
            // 
            // btnKick
            // 
            this.btnKick.BackColor = System.Drawing.Color.Tomato;
            this.btnKick.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnKick.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnKick.Location = new System.Drawing.Point(65, 425);
            this.btnKick.Name = "btnKick";
            this.btnKick.Size = new System.Drawing.Size(161, 50);
            this.btnKick.TabIndex = 11;
            this.btnKick.Text = "Ngắt kết nối";
            this.btnKick.UseVisualStyleBackColor = false;
            this.btnKick.Click += new System.EventHandler(this.btnKick_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.groupBox1.Controls.Add(this.lblStatus);
            this.groupBox1.Controls.Add(this.lblIP);
            this.groupBox1.Controls.Add(this.lblPort);
            this.groupBox1.Controls.Add(this.lblTime);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(513, 98);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(404, 264);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Thông tin chi tiết";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(11, 209);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(114, 26);
            this.lblStatus.TabIndex = 14;
            this.lblStatus.Text = "Trạng thái:";
            // 
            // lblIP
            // 
            this.lblIP.AutoSize = true;
            this.lblIP.Location = new System.Drawing.Point(12, 62);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(112, 26);
            this.lblIP.TabIndex = 11;
            this.lblIP.Text = "Địa chỉ IP:";
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(12, 109);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(140, 26);
            this.lblPort.TabIndex = 12;
            this.lblPort.Text = "Cổng kết nối:";
            this.lblPort.Click += new System.EventHandler(this.lblPort_Click);
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(10, 160);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(147, 26);
            this.lblTime.TabIndex = 13;
            this.lblTime.Text = "Thời gian vào:";
            // 
            // lstClients
            // 
            this.lstClients.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.lstClients.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstClients.FormattingEnabled = true;
            this.lstClients.ItemHeight = 26;
            this.lstClients.Location = new System.Drawing.Point(12, 98);
            this.lstClients.Name = "lstClients";
            this.lstClients.Size = new System.Drawing.Size(471, 264);
            this.lstClients.TabIndex = 9;
            this.lstClients.Click += new System.EventHandler(this.lstClients_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label1.Location = new System.Drawing.Point(237, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(477, 31);
            this.label1.TabIndex = 8;
            this.label1.Text = "DANH SÁCH CLIENT ĐANG KẾT NỐI";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnBackToMenu
            // 
            this.btnBackToMenu.BackColor = System.Drawing.SystemColors.Info;
            this.btnBackToMenu.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBackToMenu.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnBackToMenu.Location = new System.Drawing.Point(12, 12);
            this.btnBackToMenu.Name = "btnBackToMenu";
            this.btnBackToMenu.Size = new System.Drawing.Size(109, 52);
            this.btnBackToMenu.TabIndex = 7;
            this.btnBackToMenu.Text = "Quay lại";
            this.btnBackToMenu.UseVisualStyleBackColor = false;
            this.btnBackToMenu.Click += new System.EventHandler(this.btnBackToMenu_Click);
            // 
            // GSClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(929, 530);
            this.Controls.Add(this.panel1);
            this.Name = "GSClient";
            this.Text = "GSClient";
            this.Load += new System.EventHandler(this.GSClient_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnBackToMenu;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lstClients;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblIP;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnBuzz;
        private System.Windows.Forms.Button btnKick;
        private System.Windows.Forms.Label lblTotalClients;
    }
}