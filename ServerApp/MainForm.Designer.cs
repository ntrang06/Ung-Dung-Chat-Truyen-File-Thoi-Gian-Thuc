namespace ServerApp
{
    partial class MainForm
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
            this.pnlMenu = new System.Windows.Forms.Panel();
            this.btnBackToConfig = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnGoToFiles = new System.Windows.Forms.Button();
            this.btnGoToLog = new System.Windows.Forms.Button();
            this.btnGoToChat = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMenu
            // 
            this.pnlMenu.BackColor = System.Drawing.SystemColors.Info;
            this.pnlMenu.Controls.Add(this.btnBackToConfig);
            this.pnlMenu.Controls.Add(this.label2);
            this.pnlMenu.Controls.Add(this.btnGoToFiles);
            this.pnlMenu.Controls.Add(this.btnGoToLog);
            this.pnlMenu.Controls.Add(this.btnGoToChat);
            this.pnlMenu.Controls.Add(this.label1);
            this.pnlMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMenu.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pnlMenu.Location = new System.Drawing.Point(0, 0);
            this.pnlMenu.Name = "pnlMenu";
            this.pnlMenu.Size = new System.Drawing.Size(639, 450);
            this.pnlMenu.TabIndex = 0;
            // 
            // btnBackToConfig
            // 
            this.btnBackToConfig.BackColor = System.Drawing.SystemColors.Info;
            this.btnBackToConfig.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBackToConfig.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnBackToConfig.Location = new System.Drawing.Point(12, 12);
            this.btnBackToConfig.Name = "btnBackToConfig";
            this.btnBackToConfig.Size = new System.Drawing.Size(109, 52);
            this.btnBackToConfig.TabIndex = 5;
            this.btnBackToConfig.Text = "Quay lại";
            this.btnBackToConfig.UseVisualStyleBackColor = false;
            this.btnBackToConfig.Click += new System.EventHandler(this.btnBackToConfig_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(128, 350);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(408, 30);
            this.label2.TabIndex = 4;
            this.label2.Text = "Rất vui được gặp bạn! Server đã sẵn sàng";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnGoToFiles
            // 
            this.btnGoToFiles.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnGoToFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGoToFiles.Location = new System.Drawing.Point(161, 269);
            this.btnGoToFiles.Name = "btnGoToFiles";
            this.btnGoToFiles.Size = new System.Drawing.Size(298, 51);
            this.btnGoToFiles.TabIndex = 3;
            this.btnGoToFiles.Text = "Quản lý File";
            this.btnGoToFiles.UseVisualStyleBackColor = false;
            this.btnGoToFiles.Click += new System.EventHandler(this.btnQuanLyFile_Click);
            // 
            // btnGoToLog
            // 
            this.btnGoToLog.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnGoToLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGoToLog.Location = new System.Drawing.Point(161, 178);
            this.btnGoToLog.Name = "btnGoToLog";
            this.btnGoToLog.Size = new System.Drawing.Size(298, 53);
            this.btnGoToLog.TabIndex = 2;
            this.btnGoToLog.Text = "Giám sát Client ";
            this.btnGoToLog.UseVisualStyleBackColor = false;
            this.btnGoToLog.Click += new System.EventHandler(this.btnGiamSat_Click);
            // 
            // btnGoToChat
            // 
            this.btnGoToChat.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnGoToChat.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGoToChat.Location = new System.Drawing.Point(161, 92);
            this.btnGoToChat.Name = "btnGoToChat";
            this.btnGoToChat.Size = new System.Drawing.Size(298, 56);
            this.btnGoToChat.TabIndex = 1;
            this.btnGoToChat.Text = "Khung chat hệ thống ";
            this.btnGoToChat.UseVisualStyleBackColor = false;
            this.btnGoToChat.Click += new System.EventHandler(this.btnKhungChat_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.label1.Location = new System.Drawing.Point(271, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 31);
            this.label1.TabIndex = 0;
            this.label1.Text = "MENU";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(639, 450);
            this.Controls.Add(this.pnlMenu);
            this.Name = "MainForm";
            this.Text = "Main";
            this.pnlMenu.ResumeLayout(false);
            this.pnlMenu.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlMenu;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnGoToFiles;
        private System.Windows.Forms.Button btnGoToLog;
        private System.Windows.Forms.Button btnGoToChat;
        private System.Windows.Forms.Button btnBackToConfig;
        private System.Windows.Forms.Label label2;
    }
}