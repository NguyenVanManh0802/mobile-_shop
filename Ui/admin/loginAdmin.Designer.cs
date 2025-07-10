namespace MobileShop.Ui.admin
{
    partial class loginAdmin
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.guna2ButtonLogin = new Guna.UI2.WinForms.Guna2Button();
            this.guna2TextBoxPassw = new Guna.UI2.WinForms.Guna2TextBox();
            this.guna2TextBoxUserName = new Guna.UI2.WinForms.Guna2TextBox();
            this.guna2btnQuayLai = new Guna.UI2.WinForms.Guna2Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.guna2btnQuayLai);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Location = new System.Drawing.Point(4, 4);
            this.panel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(446, 606);
            this.panel1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::MobileShop.Properties.Resources.img_01;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Location = new System.Drawing.Point(96, 94);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(302, 333);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(582, 120);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(176, 32);
            this.label1.TabIndex = 1;
            this.label1.Text = "Admin Login";
            // 
            // guna2ButtonLogin
            // 
            this.guna2ButtonLogin.BorderRadius = 25;
            this.guna2ButtonLogin.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2ButtonLogin.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2ButtonLogin.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2ButtonLogin.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2ButtonLogin.FillColor = System.Drawing.Color.LimeGreen;
            this.guna2ButtonLogin.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2ButtonLogin.ForeColor = System.Drawing.Color.White;
            this.guna2ButtonLogin.Location = new System.Drawing.Point(544, 384);
            this.guna2ButtonLogin.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.guna2ButtonLogin.Name = "guna2ButtonLogin";
            this.guna2ButtonLogin.Size = new System.Drawing.Size(262, 54);
            this.guna2ButtonLogin.TabIndex = 6;
            this.guna2ButtonLogin.Text = "LOGIN";
            this.guna2ButtonLogin.Click += new System.EventHandler(this.guna2ButtonLogin_Click);
            // 
            // guna2TextBoxPassw
            // 
            this.guna2TextBoxPassw.BorderRadius = 25;
            this.guna2TextBoxPassw.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.guna2TextBoxPassw.DefaultText = "";
            this.guna2TextBoxPassw.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.guna2TextBoxPassw.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.guna2TextBoxPassw.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.guna2TextBoxPassw.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.guna2TextBoxPassw.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2TextBoxPassw.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2TextBoxPassw.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2TextBoxPassw.IconLeft = global::MobileShop.Properties.Resources.icons8_password_24;
            this.guna2TextBoxPassw.IconLeftOffset = new System.Drawing.Point(20, 0);
            this.guna2TextBoxPassw.Location = new System.Drawing.Point(544, 289);
            this.guna2TextBoxPassw.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.guna2TextBoxPassw.Name = "guna2TextBoxPassw";
            this.guna2TextBoxPassw.PlaceholderText = "Password";
            this.guna2TextBoxPassw.SelectedText = "";
            this.guna2TextBoxPassw.Size = new System.Drawing.Size(262, 44);
            this.guna2TextBoxPassw.TabIndex = 5;
            this.guna2TextBoxPassw.TextOffset = new System.Drawing.Point(10, 0);
            // 
            // guna2TextBoxUserName
            // 
            this.guna2TextBoxUserName.BorderRadius = 25;
            this.guna2TextBoxUserName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.guna2TextBoxUserName.DefaultText = "";
            this.guna2TextBoxUserName.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.guna2TextBoxUserName.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.guna2TextBoxUserName.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.guna2TextBoxUserName.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.guna2TextBoxUserName.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2TextBoxUserName.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2TextBoxUserName.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2TextBoxUserName.IconLeft = global::MobileShop.Properties.Resources.icons8_username_24;
            this.guna2TextBoxUserName.IconLeftOffset = new System.Drawing.Point(20, 0);
            this.guna2TextBoxUserName.Location = new System.Drawing.Point(544, 203);
            this.guna2TextBoxUserName.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.guna2TextBoxUserName.Name = "guna2TextBoxUserName";
            this.guna2TextBoxUserName.Padding = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.guna2TextBoxUserName.PlaceholderText = "UserName";
            this.guna2TextBoxUserName.SelectedText = "";
            this.guna2TextBoxUserName.Size = new System.Drawing.Size(262, 46);
            this.guna2TextBoxUserName.TabIndex = 4;
            this.guna2TextBoxUserName.TextOffset = new System.Drawing.Point(10, 0);
            // 
            // guna2btnQuayLai
            // 
            this.guna2btnQuayLai.BorderRadius = 20;
            this.guna2btnQuayLai.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2btnQuayLai.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2btnQuayLai.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2btnQuayLai.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2btnQuayLai.FillColor = System.Drawing.Color.Chartreuse;
            this.guna2btnQuayLai.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2btnQuayLai.ForeColor = System.Drawing.Color.Black;
            this.guna2btnQuayLai.Location = new System.Drawing.Point(7, 19);
            this.guna2btnQuayLai.Margin = new System.Windows.Forms.Padding(2);
            this.guna2btnQuayLai.Name = "guna2btnQuayLai";
            this.guna2btnQuayLai.Size = new System.Drawing.Size(106, 23);
            this.guna2btnQuayLai.TabIndex = 15;
            this.guna2btnQuayLai.Text = "Back";
            this.guna2btnQuayLai.Click += new System.EventHandler(this.guna2btnQuayLai_Click);
            // 
            // loginAdmin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(854, 634);
            this.Controls.Add(this.guna2ButtonLogin);
            this.Controls.Add(this.guna2TextBoxPassw);
            this.Controls.Add(this.guna2TextBoxUserName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "loginAdmin";
            this.Text = "loginAdmin";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2TextBox guna2TextBoxUserName;
        private Guna.UI2.WinForms.Guna2TextBox guna2TextBoxPassw;
        private Guna.UI2.WinForms.Guna2Button guna2ButtonLogin;
        private Guna.UI2.WinForms.Guna2Button guna2btnQuayLai;
    }
}