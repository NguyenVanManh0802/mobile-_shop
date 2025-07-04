using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileShop.models
{
    internal class Mobile
    {
        // IMEI thường là một chuỗi ký tự dài, không phải là số để tính toán
        public string ImeiNo { get; set; }
        public string ModelId { get; set; } // Khóa ngoại tham chiếu đến Model
        public string Status { get; set; }
        public DateTime Warranty { get; set; }
        public float Price { get; set; }
        public string Image { get; set; }
    }
}
