namespace Data_Entry_App
{
    partial class Home
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
            this.BtnLogout = new System.Windows.Forms.Button();
            this.btn_CheckFile = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BtnLogout
            // 
            this.BtnLogout.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnLogout.ForeColor = System.Drawing.Color.Black;
            this.BtnLogout.Location = new System.Drawing.Point(690, 12);
            this.BtnLogout.Name = "BtnLogout";
            this.BtnLogout.Size = new System.Drawing.Size(98, 27);
            this.BtnLogout.TabIndex = 6;
            this.BtnLogout.Text = "Exit";
            this.BtnLogout.UseVisualStyleBackColor = true;
            this.BtnLogout.Click += new System.EventHandler(this.BtnLogout_Click);
            // 
            // btn_CheckFile
            // 
            this.btn_CheckFile.Location = new System.Drawing.Point(13, 11);
            this.btn_CheckFile.Margin = new System.Windows.Forms.Padding(4);
            this.btn_CheckFile.Name = "btn_CheckFile";
            this.btn_CheckFile.Size = new System.Drawing.Size(131, 28);
            this.btn_CheckFile.TabIndex = 17;
            this.btn_CheckFile.Text = "Admin Setting";
            this.btn_CheckFile.UseVisualStyleBackColor = true;
            this.btn_CheckFile.Click += new System.EventHandler(this.BtnAdminSetting);
            // 
            // Home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btn_CheckFile);
            this.Controls.Add(this.BtnLogout);
            this.Name = "Home";
            this.Text = "Home Page";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnLogout;
        private System.Windows.Forms.Button btn_CheckFile;
    }
}