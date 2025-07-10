using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using MobileShop.dataAccessLayer;
using MobileShop.dataAccessLayer.admin;
using MobileShop.models;
using MobileShop.Ui.Employee;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using MobileShop.dataAccessLayer.employee;
using System.Reflection;



namespace MobileShop.Ui.Employee
{
    public partial class HomePageEmployee : Form
    {
        private User currentEmployee;

        // Constructor nhận Employee đã đăng nhập
        public HomePageEmployee(User loggedInEmployee)
        {
            InitializeComponent();
            currentEmployee = loggedInEmployee;
            ShowEmployeeInfo();
            LoadMobilesToPanel(); // ← Hiển thị sản phẩm
            LoadImeinoToComboBox();
        }
        private void MoFormConfirm(Mobile mobile,string modelNum)
        {
            ConfirmDetails confirmForm = new ConfirmDetails(mobile, modelNum);
            confirmForm.Show();
        }
        private void LoadImeinoToComboBox()
        {
            string connectionString = "Data Source = NGUYENMANH; Initial Catalog = mobileShop; Integrated Security = True";
            string query = "SELECT DISTINCT imeino FROM tbl_sales"; // Dùng đúng tên bảng

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            cmbImeino.Items.Clear(); // Xoá dữ liệu cũ
                            while (reader.Read())
                            {
                                cmbImeino.Items.Add(reader["imeino"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải imeino: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void LoadImeinoList()
        {
            string connectionString = "Data Source = NGUYENMANH; Initial Catalog = mobileShop; Integrated Security = True";
            string query = "SELECT DISTINCT imeino FROM tbl_sales";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            cmbImeino.Items.Clear(); // Xóa dữ liệu cũ
                            int count = 0;

                            while (reader.Read())
                            {
                                string imeino = reader["imeino"].ToString();
                                cmbImeino.Items.Add(imeino);
                                count++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu dữ liệu: " + ex.Message + "\nChi tiết lỗi có thể do SQL: " + query);
            }
        }

        private void ShowEmployeeInfo()
        {
            if (currentEmployee != null && !string.IsNullOrEmpty(currentEmployee.UserName))
            {
                lblTen.Text = $"Xin chào: {currentEmployee.UserName}";
            }
            else
            {
                lblTen.Text = "Xin chào nhân viên";
            }
        }



        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Bạn có chắc chắn muốn đăng xuất?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Hide();
                var loginForm = new LoginEmployee();
                loginForm.Show();
            }
        }


        private void LoadMobilesToPanel()

        {
            flowLayoutPanelProducts.Controls.Clear(); // Xóa dữ liệu cũ
            MobileDAL mobileDAL = new MobileDAL();
            ModelData model = new ModelData();
            List<Mobile> mobileList = mobileDAL.GetAllMobiles();
            


            foreach (Mobile mobile in mobileList)

            {

                Panel panel = new Panel();

                panel.Size = new Size(300, 150);

                panel.BorderStyle = BorderStyle.FixedSingle;

                panel.Cursor = Cursors.Hand;


                


                Label lblModel = new Label();

                lblModel.Text = $"Model: {mobile.ModelId}";

                lblModel.Location = new Point(10, 10);

                lblModel.AutoSize = true;


                String modelNum = model.GetModelNumById(mobile.ModelId);

                Label lblModelNum = new Label();

                lblModelNum.Text = $"Tên sản phẩm: {modelNum}";

                lblModelNum.Location = new Point(10, 35);

                lblModelNum.AutoSize = true;


                Label lblPrice = new Label();

                lblPrice.Text = $"Giá: {mobile.Price} VND";

                lblPrice.Location = new Point(10, 60);

                lblPrice.AutoSize = true;

                PictureBox pic = new PictureBox();
                pic.Size = new Size(80, 80);
                pic.Location = new Point(200, 10);
                pic.SizeMode = PictureBoxSizeMode.Zoom;
                if (File.Exists(mobile.Image))
                    pic.Image = Image.FromFile(mobile.Image);
                else
                    pic.BackColor = Color.Gray;

                // Bắt sự kiện click cho mọi thành phần

                panel.Click += (s, e) => MoFormConfirm(mobile,modelNum);

                lblModel.Click += (s, e) => MoFormConfirm(mobile,modelNum);

                lblPrice.Click += (s, e) => MoFormConfirm(mobile, modelNum);

                pic.Click += (s, e) => MoFormConfirm(mobile, modelNum);



                panel.Controls.Add(lblModel);
                panel.Controls.Add(lblModelNum);

                panel.Controls.Add(lblPrice);

                panel.Controls.Add(pic);



                flowLayoutPanelProducts.Controls.Add(panel);

            }



            panelMainContent.BringToFront();

        }


        // ✅ Đặt HÀM này bên ngoài LoadMobilesToPanel
        private void AddClickEvent(Control c, Mobile mobileData,string modelNum)
        {
            c.Click += (s, e) =>
            {
                ConfirmDetails confirmForm = new ConfirmDetails(mobileData, modelNum);
                confirmForm.ShowDialog();
            };

            foreach (Control child in c.Controls)
            {
                AddClickEvent(child, mobileData, modelNum);
            }
        }




        // Giữ Paint handler nếu đã đăng ký sự kiện
        private void flowLayoutPanelProducts_Paint(object sender, PaintEventArgs e)
        {
            
            // Tránh load lại nhiều lần
            if (flowLayoutPanelProducts.Controls.Count > 0)

                return;

            // Lấy dữ liệu
            MobileDAL mobileDAL = new MobileDAL();
            List<Mobile> mobileList = mobileDAL.GetAllMobiles();
            ModelData modelData = new ModelData();  


            foreach (Mobile mobile in mobileList)
            {

                Panel panel = new Panel();
                panel.Size = new Size(300, 150);
                panel.BorderStyle = BorderStyle.FixedSingle;

                Label lblModel = new Label();
                lblModel.Text = $"Model: {mobile.ModelId}";
                lblModel.Location = new Point(10, 10);
                lblModel.AutoSize = true;


                String modelNum = modelData.GetModelNumById(mobile.ModelId);
                Label lblModelNum = new Label();
                lblModelNum.Text = $"Tên sản phẩm: {modelNum}";
                lblModelNum.Location = new Point(10, 35);
                lblModelNum.AutoSize = true;

                Label lblPrice = new Label();
                lblPrice.Text = $"Giá: {mobile.Price} VND";
                lblPrice.Location = new Point(10, 60);
                lblPrice.AutoSize = true;

                PictureBox pic = new PictureBox();
                pic.Size = new Size(80, 80);
                pic.Location = new Point(200, 10);
                pic.SizeMode = PictureBoxSizeMode.Zoom;

                if (File.Exists(mobile.Image))
                {
                    pic.Image = Image.FromFile(mobile.Image);
                }
                else
                {
                    MessageBox.Show("Ảnh không tồn tại: " + mobile.Image); // DEBUG FLAG 3
                    pic.BackColor = Color.Gray;
                }

                // DEBUG FLAG 4 – Khi click
                panel.Click += (s, evt1) => {
                    MoFormConfirm(mobile, modelNum);
                };
                lblModel.Click += (s, evt2) => {
                    MoFormConfirm(mobile, modelNum);
                };
                lblPrice.Click += (s, evt3) => {
                    MoFormConfirm(mobile, modelNum);
                };
                pic.Click += (s, evt4) => {

                    MoFormConfirm(mobile, modelNum);
                };

                panel.Controls.Add(lblModel);
                panel.Controls.Add(lblModelNum);
                panel.Controls.Add(lblPrice);
                panel.Controls.Add(pic);

                flowLayoutPanelProducts.Controls.Add(panel);
            }

        }


        public HomePageEmployee()
        {
            InitializeComponent();
            // Có thể để currentEmployee = null hoặc xử lý mặc định
        }

       

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            LoadImeinoList();
            panelSearchIMEI.BringToFront(); // đưa panel lên đầu
            panelSearchIMEI.Visible = true; // hiển thị panel tìm kiếm
        }

        private void cmbImeino_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedImeino = cmbImeino.SelectedItem.ToString();

            string connectionString = "Data Source=NGUYENMANH;Initial Catalog=mobileShop;Integrated Security=True";
            string query = @"
        SELECT c.cusName, c.numphone, c.email, c.address
        FROM tbl_sales s
        JOIN tbl_customer c ON s.cusId = c.customerId
        WHERE s.imeino = @imeino";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@imeino", selectedImeino);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                lblCustomerName.Text = "Tên KH: " + reader["cusName"].ToString();
                                lblPhoneNumber.Text = "SĐT: " + reader["numphone"].ToString();
                                lblEmail.Text = "Email: " + reader["email"].ToString();
                                lblAddress.Text = "Địa chỉ: " + reader["address"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy thông tin khách hàng cho IMEINO này.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm khách hàng: " + ex.Message);
            }
        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }

        private void lblCustomerInfo_Click(object sender, EventArgs e)
        {

        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            LoadMobilesToPanel();
            panelMainContent.BringToFront();
            panelSearchIMEI.Visible = false; // nếu bạn đang dùng panel tìm kiếm
        }

        private void btnDangXuat_Click_1(object sender, EventArgs e)
        {
            this.Hide(); // Ẩn form hiện tại
            LoginEmployee loginForm = new LoginEmployee(); // Tạo form đăng nhập
            loginForm.Show(); // Hiển thị form đăng nhập
        }
    }
}