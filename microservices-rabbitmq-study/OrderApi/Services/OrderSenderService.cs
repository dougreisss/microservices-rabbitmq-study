using Order.WebApi.Services.Interface;
using RabbitMQ.Client;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Extensions.Options;

namespace Order.WebApi.Services
{
    public class OrderSenderService : IOrderSenderService
    {
        private readonly IOptions<Model.OrderSenderConfig> _orderSenderConfig;

        public OrderSenderService(IOptions<Model.OrderSenderConfig> orderSenderConfig)
        {
            _orderSenderConfig = orderSenderConfig;
        }

        public async Task OrderSender(Model.Order order)
        {
            string queue = _orderSenderConfig.Value.Queue ?? "";

            var factory = new ConnectionFactory() 
            {
                HostName = _orderSenderConfig.Value.Hostname ?? "",
                UserName = _orderSenderConfig.Value.Username ?? "",
                Password = _orderSenderConfig.Value.Password ?? ""
            };

            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: queue, 
                durable: true,
                exclusive: false, 
                autoDelete: false, 
                arguments: null);

            string orderJson = JsonConvert.SerializeObject(order);

            var body = Encoding.UTF8.GetBytes(orderJson);

            await channel.BasicPublishAsync(exchange: string.Empty, routingKey: queue, body: body);
        }
    }
}
