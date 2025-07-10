using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileShop.models
{
    internal class Transaction
    {
        public string TransId { get; set; }
        public string ModelId { get; set; } // Khóa ngoại tham chiếu đến Model
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
        public float Amount { get; set; }
        public string TransactionType { get; set; } 
    }
}
