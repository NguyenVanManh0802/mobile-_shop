using MobileShop.Ui.admin;
using MobileShop.Ui.Employee;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MobileShop.Ui
{
    public partial class HomePage : Form
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            loginAdmin loginAdmin = new loginAdmin();   
            loginAdmin.Show();
            this.Hide();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            LoginEmployee   loginEmployee = new LoginEmployee();
            loginEmployee.Show();
            this.Hide();
        }
    }
}
