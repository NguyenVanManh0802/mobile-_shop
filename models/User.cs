using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileShop.models
{
    public class User
    {
        public string userId {  get; set; } 
        public string UserName { get; set; }
        public string email { get; set; }
        public string Password { get; set; } // Đổi từ PWD để rõ nghĩa hơn
        public string numberPhone { get; set; }
        public string Address { get; set; }
        public string fullName { get; set; }
    }
}
