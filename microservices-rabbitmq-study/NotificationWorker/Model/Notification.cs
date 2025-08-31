using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationWorker.Model
{
    public class Notification
    {
        public string ClientName { get; set; }
        public string ClientEmail { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int QuantityProduct { get; set; } 
        public decimal TotalAmount => UnitPrice * QuantityProduct;
    }
}
