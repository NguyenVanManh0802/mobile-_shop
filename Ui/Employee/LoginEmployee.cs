using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MobileShop.Ui.Employee
{
    public partial class LoginEmployee : Form
    {
        public LoginEmployee()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void linklblCreateYourAccount_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();

            // Mở form đăng ký
            RegisterEmployee registerForm = new RegisterEmployee();
            registerForm.FormClosed += (s, args) => this.Show(); // Hiện lại Login sau khi Register đóng (nếu muốn)
            registerForm.Show();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUserName.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = "Data Source = NGUYENMANH; Initial Catalog = mobileShop; Integrated Security = True";
                
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM tbl_user WHERE Username = @Username AND password = @Password";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);

                    try
                    {
                        conn.Open();
                        int count = (int)cmd.ExecuteScalar();

                        if (count > 0)
                        {
                            MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Chuyển sang form HomePageEmployee
                            HomePageEmployee homePage = new HomePageEmployee();
                            homePage.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void linklblForgetPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            forgot_password forgotForm = new forgot_password();
            forgotForm.Show();

            // Ẩn form đăng nhập hiện tại
            this.Hide();
        }

        private void guna2btnQuayLai_Click(object sender, EventArgs e)
        {
            this.Hide(); // Ẩn form hiện tại
            HomePage homepage = new HomePage(); // Tạo form đăng nhập
            homepage.Show(); // Hiển thị form đăng nhập
        }
    }
}
