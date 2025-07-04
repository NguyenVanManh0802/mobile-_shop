using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MobileShop.Ui.admin
{
    public partial class loginAdmin : Form
    {
        public loginAdmin()
        {
            InitializeComponent();
        }

        private void guna2ButtonLogin_Click(object sender, EventArgs e)
        {
            string userName = guna2TextBoxUserName.Text;
            string password = guna2TextBoxPassw.Text;

            if (userName == "admin" && password == "12345")
            {
                // Đăng nhập thành công: Mở trang chủ và ẩn form hiện tại
                HompageAdmin hompage = new HompageAdmin();
                hompage.Show();
                this.Hide();
            }
            else
            {
                // Đăng nhập không thành công: Hiển thị hộp thoại thông báo lỗi
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng.",
                                "Lỗi đăng nhập",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }
    }
}
