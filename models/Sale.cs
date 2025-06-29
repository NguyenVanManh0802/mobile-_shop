using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileShop.models
{
    internal class Sale
    {
        public string SlsId { get; set; }
        public string ImeiNo { get; set; } // Khóa ngoại tham chiếu đến Mobile
        public DateTime PurchaseDate { get; set; } // Sửa lỗi chính tả từ "PurchageDate"
        public float Price { get; set; }
        public string CustId { get; set; } // Khóa ngoại tham chiếu đến Customer
    }
}
