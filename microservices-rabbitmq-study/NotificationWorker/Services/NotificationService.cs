using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationConsumer.Services.Interfaces;
using NotificationWorker.Model;

namespace NotificationConsumer.Services
{
    public class NotificationService : INotificationService
    {
        public void ExecuteNotification(Notification notification)
        {
            Console.WriteLine(
                $"Client: {notification.ClientName} " +
                $"({notification.ClientEmail}) purchased {notification.QuantityProduct} " +
                $"unit(s) of the product {notification.ProductName} at {notification.UnitPrice:C} each, " +
                $"totaling {notification.TotalAmount:C}."
            );
        }
    }
}
