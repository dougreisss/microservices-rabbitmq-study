using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentConsumer.Model;

namespace PaymentConsumer.Services.Interfaces
{
    public interface IPaymentService
    {
        bool ExecutePayment(Order order);
    }
}
