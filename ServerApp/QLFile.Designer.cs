namespace ServerApp
{
    partial class QLFile
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
        /// Required method for Designer modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnChooseSaveLocation = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnDownload = new System.Windows.Forms.Button();
            this.lvFiles = new System.Windows.Forms.ListView();
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.txtCurrentPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBackToMenu = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Info;
            this.panel1.Controls.Add(this.btnChooseSaveLocation);
            this.panel1.Controls.Add(this.btnRefresh);
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Controls.Add(this.btnDownload);
            this.panel1.Controls.Add(this.lvFiles);
            this.panel1.Controls.Add(this.txtCurrentPath);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btnBackToMenu);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(865, 458);
            this.panel1.TabIndex = 0;
            // 
            // btnChooseSaveLocation
            // 
            this.btnChooseSaveLocation.BackColor = System.Drawing.SystemColors.Info;
            this.btnChooseSaveLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChooseSaveLocation.Location = new System.Drawing.Point(39, 73);
            this.btnChooseSaveLocation.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnChooseSaveLocation.Name = "btnChooseSaveLocation";
            this.btnChooseSaveLocation.Size = new System.Drawing.Size(57, 35);
            this.btnChooseSaveLocation.TabIndex = 15;
            this.btnChooseSaveLocation.Text = "...";
            this.btnChooseSaveLocation.UseVisualStyleBackColor = false;
            this.btnChooseSaveLocation.Click += new System.EventHandler(this.btnChooseSaveLocation_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.Tomato;
            this.btnRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnRefresh.Location = new System.Drawing.Point(681, 385);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(133, 46);
            this.btnRefresh.TabIndex = 14;
            this.btnRefresh.Text = "Làm mới";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.Tomato;
            this.btnDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnDelete.Location = new System.Drawing.Point(400, 385);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(140, 46);
            this.btnDelete.TabIndex = 13;
            this.btnDelete.Text = "Xóa file";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnDownload
            // 
            this.btnDownload.BackColor = System.Drawing.Color.Tomato;
            this.btnDownload.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDownload.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnDownload.Location = new System.Drawing.Point(86, 385);
            this.btnDownload.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(164, 46);
            this.btnDownload.TabIndex = 12;
            this.btnDownload.Text = "Tải file về máy";
            this.btnDownload.UseVisualStyleBackColor = false;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // lvFiles
            // 
            this.lvFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colSize,
            this.colType});
            this.lvFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvFiles.HideSelection = false;
            this.lvFiles.Location = new System.Drawing.Point(27, 138);
            this.lvFiles.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lvFiles.Name = "lvFiles";
            this.lvFiles.Size = new System.Drawing.Size(811, 237);
            this.lvFiles.TabIndex = 11;
            this.lvFiles.UseCompatibleStateImageBehavior = false;
            this.lvFiles.View = System.Windows.Forms.View.Details;
            this.lvFiles.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvFiles_MouseDoubleClick);
            // 
            // colName
            // 
            this.colName.Text = "Tên File / Thư mục";
            this.colName.Width = 487;
            // 
            // colSize
            // 
            this.colSize.Text = "Kích thước";
            this.colSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colSize.Width = 144;
            // 
            // colType
            // 
            this.colType.Text = "Định dạng";
            this.colType.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colType.Width = 120;
            // 
            // txtCurrentPath
            // 
            this.txtCurrentPath.Location = new System.Drawing.Point(113, 86);
            this.txtCurrentPath.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtCurrentPath.Name = "txtCurrentPath";
            this.txtCurrentPath.Size = new System.Drawing.Size(725, 22);
            this.txtCurrentPath.TabIndex = 10;
            this.txtCurrentPath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCurrentPath_KeyDown);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label1.Location = new System.Drawing.Point(281, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(287, 26);
            this.label1.TabIndex = 9;
            this.label1.Text = "HỆ THỐNG QUẢN LÝ FILE";
            this.label1.UseMnemonic = false;
            // 
            // btnBackToMenu
            // 
            this.btnBackToMenu.BackColor = System.Drawing.SystemColors.Info;
            this.btnBackToMenu.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBackToMenu.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnBackToMenu.Location = new System.Drawing.Point(11, 10);
            this.btnBackToMenu.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnBackToMenu.Name = "btnBackToMenu";
            this.btnBackToMenu.Size = new System.Drawing.Size(97, 42);
            this.btnBackToMenu.TabIndex = 8;
            this.btnBackToMenu.Text = "Quay lại";
            this.btnBackToMenu.UseVisualStyleBackColor = false;
            this.btnBackToMenu.Click += new System.EventHandler(this.btnBackToMenu_Click);
            // 
            // QLFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(865, 458);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "QLFile";
            this.Text = "QLFile";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.QLFile_FormClosing);
            this.Load += new System.EventHandler(this.QLFile_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnBackToMenu;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCurrentPath;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Button btnChooseSaveLocation;
        private System.Windows.Forms.ListView lvFiles;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colSize;
        private System.Windows.Forms.ColumnHeader colType;
    }
}