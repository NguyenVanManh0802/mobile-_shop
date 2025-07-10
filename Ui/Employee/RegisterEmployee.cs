using MobileShop.dataAccessLayer.admin;
using MobileShop.models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MobileShop.Ui.Employee
{   
    public partial class RegisterEmployee : Form
    {
        public RegisterEmployee()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void guna2ButtonLogin_Click(object sender, EventArgs e)
        {

        }

        private void txtAddress_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            string username = txtUserName.Text.Trim();
            string password = txtPassword.Text.Trim();
            string email = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string address = txtAddress.Text.Trim();
            string fullname = txtFullName.Text.Trim();

            // Kiểm tra dữ liệu trống
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(phone) ||
                string.IsNullOrWhiteSpace(address) || string.IsNullOrWhiteSpace(fullname))
            {
                MessageBox.Show("Please fill in all fields.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            EmployeeData employeeData = new EmployeeData();

            // Kiểm tra Username
            if (!employeeData.IsUsernameUnique(username))
            {
                MessageBox.Show("Username already exists. Please choose another.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra Email
            if (!employeeData.IsEmailUnique(email))
            {
                MessageBox.Show("Email already exists. Please choose another.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Tạo đối tượng User
            User newUser = new User
            {
                UserName = username,
                Password = password,
                email = email,
                numberPhone = phone,
                Address = address,
                fullName = fullname
            };

            // Thêm vào DB
            bool isSuccess = employeeData.AddEmployee(newUser);

            if (isSuccess)
            {
                MessageBox.Show("Register successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Quay lại LoginEmployee
                LoginEmployee loginForm = new LoginEmployee();
                loginForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Register failed. Please try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Mở form đăng nhập
            LoginEmployee loginForm = new LoginEmployee();
            loginForm.Show();

            // Đóng (ẩn) form hiện tại - là form đăng ký
            this.Hide();
        }
    }
}
