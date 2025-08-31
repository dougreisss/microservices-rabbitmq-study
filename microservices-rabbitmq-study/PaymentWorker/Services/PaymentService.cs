using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentConsumer.Model;
using PaymentConsumer.Services.Interfaces;

namespace PaymentConsumer.Services
{
    public class PaymentService : IPaymentService
    {
        public bool ExecutePayment(Order order)
        {
            return true;
        }
    }
}
