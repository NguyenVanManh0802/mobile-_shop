using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileShop.models
{
    internal class Customer
    {
        public int CustId { get; set; }
        public string CustomerName { get; set; } // Đổi từ "Cust Name"
        public string MobileNumber { get; set; }
        public string EmailId { get; set; }
        public string Address { get; set; }
    }
}
