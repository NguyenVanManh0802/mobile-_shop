using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileShop.models
{
    internal class User
    {
        public string UserName { get; set; }
        public string email { get; set; }
        public string Password { get; set; } // Đổi từ PWD để rõ nghĩa hơn
        public string EmployeeName { get; set; }
        public string Address { get; set; }
        public string MobileNumber { get; set; }
    }
}
