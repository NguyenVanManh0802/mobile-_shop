using System.Windows.Forms;

namespace MobileShop.Ui.Employee
{
    partial class HomePageEmployee
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>

        private void LoadUserControl(UserControl uc)
        {

            uc.Dock = DockStyle.Fill;          // Cho nó chiếm toàn bộ panel

        }

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HomePageEmployee));
            this.lblTen = new System.Windows.Forms.Label();
            this.panelTieuDe = new System.Windows.Forms.Panel();
            this.btnHome = new Guna.UI2.WinForms.Guna2Button();
            this.btnTimKiem = new Guna.UI2.WinForms.Guna2Button();
            this.btnDangXuat = new Guna.UI2.WinForms.Guna2Button();
            this.picturelogo = new System.Windows.Forms.PictureBox();
            this.lBLTenTrangChu = new System.Windows.Forms.Label();
            this.panelMainContent = new System.Windows.Forms.Panel();
            this.flowLayoutPanelProducts = new System.Windows.Forms.FlowLayoutPanel();
            this.panelSearchIMEI = new System.Windows.Forms.Panel();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblAddress = new System.Windows.Forms.Label();
            this.lblPhoneNumber = new System.Windows.Forms.Label();
            this.lblTieuDe = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.cmbImeino = new System.Windows.Forms.ComboBox();
            this.lblCustomerName = new System.Windows.Forms.Label();
            this.panelTieuDe.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picturelogo)).BeginInit();
            this.panelMainContent.SuspendLayout();
            this.panelSearchIMEI.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTen
            // 
            this.lblTen.AutoSize = true;
            this.lblTen.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTen.ForeColor = System.Drawing.Color.Blue;
            this.lblTen.Location = new System.Drawing.Point(524, 84);
            this.lblTen.Name = "lblTen";
            this.lblTen.Size = new System.Drawing.Size(214, 38);
            this.lblTen.TabIndex = 0;
            this.lblTen.Text = "TRANG CHỦ";
            // 
            // panelTieuDe
            // 
            this.panelTieuDe.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panelTieuDe.Controls.Add(this.btnHome);
            this.panelTieuDe.Controls.Add(this.btnTimKiem);
            this.panelTieuDe.Controls.Add(this.btnDangXuat);
            this.panelTieuDe.Controls.Add(this.picturelogo);
            this.panelTieuDe.Controls.Add(this.lBLTenTrangChu);
            this.panelTieuDe.Controls.Add(this.lblTen);
            this.panelTieuDe.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTieuDe.Location = new System.Drawing.Point(0, 0);
            this.panelTieuDe.Name = "panelTieuDe";
            this.panelTieuDe.Size = new System.Drawing.Size(1280, 159);
            this.panelTieuDe.TabIndex = 22;
            // 
            // btnHome
            // 
            this.btnHome.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnHome.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnHome.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnHome.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnHome.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnHome.ForeColor = System.Drawing.Color.White;
            this.btnHome.Location = new System.Drawing.Point(972, 120);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(149, 33);
            this.btnHome.TabIndex = 26;
            this.btnHome.Text = "Home";
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // btnTimKiem
            // 
            this.btnTimKiem.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnTimKiem.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnTimKiem.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnTimKiem.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnTimKiem.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnTimKiem.ForeColor = System.Drawing.Color.White;
            this.btnTimKiem.Location = new System.Drawing.Point(1144, 120);
            this.btnTimKiem.Name = "btnTimKiem";
            this.btnTimKiem.Size = new System.Drawing.Size(133, 33);
            this.btnTimKiem.TabIndex = 25;
            this.btnTimKiem.Text = "Search";
            this.btnTimKiem.Click += new System.EventHandler(this.btnTimKiem_Click);
            // 
            // btnDangXuat
            // 
            this.btnDangXuat.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnDangXuat.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnDangXuat.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnDangXuat.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnDangXuat.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnDangXuat.ForeColor = System.Drawing.Color.White;
            this.btnDangXuat.Location = new System.Drawing.Point(1144, 3);
            this.btnDangXuat.Name = "btnDangXuat";
            this.btnDangXuat.Size = new System.Drawing.Size(136, 28);
            this.btnDangXuat.TabIndex = 24;
            this.btnDangXuat.Text = "Đăng Xuất";
            this.btnDangXuat.Click += new System.EventHandler(this.btnDangXuat_Click_1);
            // 
            // picturelogo
            // 
            this.picturelogo.Image = ((System.Drawing.Image)(resources.GetObject("picturelogo.Image")));
            this.picturelogo.Location = new System.Drawing.Point(0, 0);
            this.picturelogo.Name = "picturelogo";
            this.picturelogo.Size = new System.Drawing.Size(278, 163);
            this.picturelogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picturelogo.TabIndex = 21;
            this.picturelogo.TabStop = false;
            // 
            // lBLTenTrangChu
            // 
            this.lBLTenTrangChu.AutoSize = true;
            this.lBLTenTrangChu.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lBLTenTrangChu.Location = new System.Drawing.Point(353, 18);
            this.lBLTenTrangChu.Name = "lBLTenTrangChu";
            this.lBLTenTrangChu.Size = new System.Drawing.Size(581, 38);
            this.lBLTenTrangChu.TabIndex = 22;
            this.lBLTenTrangChu.Text = "HỆ THỐNG QUẢN LÝ MOBILE SHOP";
            // 
            // panelMainContent
            // 
            this.panelMainContent.BackColor = System.Drawing.SystemColors.Control;
            this.panelMainContent.Controls.Add(this.flowLayoutPanelProducts);
            this.panelMainContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMainContent.Location = new System.Drawing.Point(0, 159);
            this.panelMainContent.Name = "panelMainContent";
            this.panelMainContent.Size = new System.Drawing.Size(1280, 809);
            this.panelMainContent.TabIndex = 23;
            // 
            // flowLayoutPanelProducts
            // 
            this.flowLayoutPanelProducts.AutoScroll = true;
            this.flowLayoutPanelProducts.BackColor = System.Drawing.SystemColors.ControlLight;
            this.flowLayoutPanelProducts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelProducts.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanelProducts.Name = "flowLayoutPanelProducts";
            this.flowLayoutPanelProducts.Size = new System.Drawing.Size(1280, 809);
            this.flowLayoutPanelProducts.TabIndex = 0;
            this.flowLayoutPanelProducts.Paint += new System.Windows.Forms.PaintEventHandler(this.flowLayoutPanelProducts_Paint);
            // 
            // panelSearchIMEI
            // 
            this.panelSearchIMEI.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelSearchIMEI.Controls.Add(this.lblEmail);
            this.panelSearchIMEI.Controls.Add(this.lblAddress);
            this.panelSearchIMEI.Controls.Add(this.lblPhoneNumber);
            this.panelSearchIMEI.Controls.Add(this.lblTieuDe);
            this.panelSearchIMEI.Controls.Add(this.cmbImeino);
            this.panelSearchIMEI.Controls.Add(this.lblCustomerName);
            this.panelSearchIMEI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSearchIMEI.Location = new System.Drawing.Point(0, 159);
            this.panelSearchIMEI.Name = "panelSearchIMEI";
            this.panelSearchIMEI.Size = new System.Drawing.Size(1280, 809);
            this.panelSearchIMEI.TabIndex = 0;
            this.panelSearchIMEI.Visible = false;
            // 
            // lblEmail
            // 
            this.lblEmail.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmail.Location = new System.Drawing.Point(81, 324);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Padding = new System.Windows.Forms.Padding(10, 5, 0, 0);
            this.lblEmail.Size = new System.Drawing.Size(518, 40);
            this.lblEmail.TabIndex = 8;
            this.lblEmail.Text = "Email";
            // 
            // lblAddress
            // 
            this.lblAddress.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddress.Location = new System.Drawing.Point(81, 263);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Padding = new System.Windows.Forms.Padding(10, 5, 0, 0);
            this.lblAddress.Size = new System.Drawing.Size(518, 40);
            this.lblAddress.TabIndex = 7;
            this.lblAddress.Text = "Địa Chỉ";
            // 
            // lblPhoneNumber
            // 
            this.lblPhoneNumber.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblPhoneNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPhoneNumber.Location = new System.Drawing.Point(81, 206);
            this.lblPhoneNumber.Name = "lblPhoneNumber";
            this.lblPhoneNumber.Padding = new System.Windows.Forms.Padding(10, 5, 0, 0);
            this.lblPhoneNumber.Size = new System.Drawing.Size(518, 40);
            this.lblPhoneNumber.TabIndex = 6;
            this.lblPhoneNumber.Text = "Số Điện Thoại :";
            // 
            // lblTieuDe
            // 
            this.lblTieuDe.AutoSize = false;
            this.lblTieuDe.BackColor = System.Drawing.Color.Transparent;
            this.lblTieuDe.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTieuDe.Location = new System.Drawing.Point(557, 33);
            this.lblTieuDe.Name = "lblTieuDe";
            this.lblTieuDe.Size = new System.Drawing.Size(145, 31);
            this.lblTieuDe.TabIndex = 2;
            this.lblTieuDe.Text = "IMENO";
            this.lblTieuDe.Click += new System.EventHandler(this.guna2HtmlLabel1_Click);
            // 
            // cmbImeino
            // 
            this.cmbImeino.FormattingEnabled = true;
            this.cmbImeino.Location = new System.Drawing.Point(139, 70);
            this.cmbImeino.Name = "cmbImeino";
            this.cmbImeino.Size = new System.Drawing.Size(911, 24);
            this.cmbImeino.TabIndex = 0;
            this.cmbImeino.SelectedIndexChanged += new System.EventHandler(this.cmbImeino_SelectedIndexChanged);
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblCustomerName.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerName.Location = new System.Drawing.Point(81, 148);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Padding = new System.Windows.Forms.Padding(10, 5, 0, 0);
            this.lblCustomerName.Size = new System.Drawing.Size(518, 40);
            this.lblCustomerName.TabIndex = 1;
            this.lblCustomerName.Text = "Tên KH :\n";
            this.lblCustomerName.Click += new System.EventHandler(this.lblCustomerInfo_Click);
            // 
            // HomePageEmployee
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1280, 968);
            this.Controls.Add(this.panelMainContent);
            this.Controls.Add(this.panelSearchIMEI);
            this.Controls.Add(this.panelTieuDe);
            this.Name = "HomePageEmployee";
            this.Text = "HomePage";
            this.panelTieuDe.ResumeLayout(false);
            this.panelTieuDe.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picturelogo)).EndInit();
            this.panelMainContent.ResumeLayout(false);
            this.panelSearchIMEI.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTen;
        private System.Windows.Forms.PictureBox picturelogo;
        private System.Windows.Forms.Panel panelTieuDe;
        private System.Windows.Forms.Label lBLTenTrangChu;
        private Panel panelMainContent;
        private FlowLayoutPanel flowLayoutPanelProducts;
        private Guna.UI2.WinForms.Guna2Button btnDangXuat;
        private Panel panelSearchIMEI;
        private Guna.UI2.WinForms.Guna2Button btnTimKiem;
        private ComboBox cmbImeino;
        private Label lblCustomerName;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblTieuDe;
        private Label lblEmail;
        private Label lblAddress;
        private Label lblPhoneNumber;
        private Guna.UI2.WinForms.Guna2Button btnHome;
    }
}