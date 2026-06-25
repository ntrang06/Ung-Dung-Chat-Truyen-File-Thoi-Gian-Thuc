namespace ServerApp
{
    partial class ServerForm
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
            this.txtPort = new System.Windows.Forms.TextBox();
            this.btnKhoiDongServer = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnDungServer = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtPort
            // 
            this.txtPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPort.Location = new System.Drawing.Point(312, 177);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(138, 32);
            this.txtPort.TabIndex = 0;
            // 
            // btnKhoiDongServer
            // 
            this.btnKhoiDongServer.BackColor = System.Drawing.SystemColors.Info;
            this.btnKhoiDongServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnKhoiDongServer.Location = new System.Drawing.Point(133, 265);
            this.btnKhoiDongServer.Name = "btnKhoiDongServer";
            this.btnKhoiDongServer.Size = new System.Drawing.Size(141, 46);
            this.btnKhoiDongServer.TabIndex = 1;
            this.btnKhoiDongServer.Text = "Khởi động";
            this.btnKhoiDongServer.UseVisualStyleBackColor = false;
            this.btnKhoiDongServer.Click += new System.EventHandler(this.btnKhoiDongServer_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(144, 183);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(130, 26);
            this.label1.TabIndex = 3;
            this.label1.Text = "Cổng (Port):";
            // 
            // btnDungServer
            // 
            this.btnDungServer.BackColor = System.Drawing.SystemColors.Info;
            this.btnDungServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDungServer.Location = new System.Drawing.Point(417, 268);
            this.btnDungServer.Name = "btnDungServer";
            this.btnDungServer.Size = new System.Drawing.Size(126, 43);
            this.btnDungServer.TabIndex = 2;
            this.btnDungServer.Text = "Dừng";
            this.btnDungServer.UseVisualStyleBackColor = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Info;
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.btnDungServer);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btnKhoiDongServer);
            this.panel1.Controls.Add(this.txtPort);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(671, 468);
            this.panel1.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(25, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(607, 26);
            this.label2.TabIndex = 4;
            this.label2.Text = "Chào mừng đến Server vui lòng nhập Port để khởi động ";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(671, 468);
            this.Controls.Add(this.panel1);
            this.Name = "ServerForm";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Button btnKhoiDongServer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnDungServer;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
    }
}

