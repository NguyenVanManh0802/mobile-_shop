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
    public partial class forgot_password : Form
    {
        public forgot_password()
        {
            InitializeComponent();
        }
        private string connectionString = "Data Source=NGUYENMANH;Initial Catalog = mobileShop; Integrated Security = True";

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void lblThongBao_Click(object sender, EventArgs e)
        {

        }

        private void lblMatKhauMoi_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void forgot_password_Load(object sender, EventArgs e)
        {

        }

        private void guna2btnQuayLai_Click(object sender, EventArgs e)
        {
            this.Hide(); // Ẩn form hiện tại
            LoginEmployee loginForm = new LoginEmployee(); // Tạo form đăng nhập
            loginForm.Show(); // Hiển thị form đăng nhập
        }

        private void guna2btnLuu_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string newPassword = txtNewPassword.Text.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(newPassword))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ email và mật khẩu mới.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Kiểm tra email tồn tại
                    string checkQuery = "SELECT COUNT(*) FROM tbl_user WHERE email = @Email";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@Email", email);
                        int count = (int)checkCmd.ExecuteScalar();

                        if (count == 0)
                        {
                            MessageBox.Show("Email này chưa được đăng ký.", "Không tìm thấy", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // Cập nhật mật khẩu mới
                    string updateQuery = "UPDATE tbl_user SET password = @NewPassword WHERE email = @Email";
                    using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                    {
                        updateCmd.Parameters.AddWithValue("@NewPassword", newPassword);
                        updateCmd.Parameters.AddWithValue("@Email", email);

                        int rowsAffected = updateCmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Cập nhật mật khẩu thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close(); // Đóng form quên mật khẩu
                            LoginEmployee login = new LoginEmployee();
                            login.Show(); // Quay về trang đăng nhập
                        }
                        else
                        {
                            MessageBox.Show("Cập nhật thất bại. Vui lòng thử lại.", "Thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi hệ thống: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void guna2btnQuayLaiDangKy_Click(object sender, EventArgs e)
        {
            this.Hide(); // Ẩn form hiện tại
            RegisterEmployee registerForm = new RegisterEmployee(); // Tạo form đăng ký
            registerForm.Show(); // Hiển thị form đăng ký
        }
    }
}
