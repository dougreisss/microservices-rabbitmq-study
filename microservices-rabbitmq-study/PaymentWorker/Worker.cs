using System.Text;
using System.Threading.Channels;
using Newtonsoft.Json;
using PaymentConsumer.Model;
using PaymentConsumer.Services.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace PaymentWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IPaymentService _paymentService;

        public Worker(ILogger<Worker> logger, IPaymentService paymentService)
        {
            _logger = logger;
            _paymentService = paymentService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Payment Worker running at: {time}", DateTimeOffset.Now);

            IChannel channel = await CreateConnection(stoppingToken);

            await channel.QueueDeclareAsync(queue: "orders", durable: true, exclusive: false,
                autoDelete: false, arguments: null);

            await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);

            _logger.LogInformation("Payment Worker - Waiting for messages - running at: {time}", DateTimeOffset.Now);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                _logger.LogInformation("Payment Worker - Received {message}- running at: {time}", message, DateTimeOffset.Now);

                Order? order = JsonConvert.DeserializeObject<Order>(message);

                if (_paymentService.ExecutePayment(order))
                {
                    // todo
                    // publish notification message

                    // here channel could also be accessed as ((AsyncEventingBasicConsumer)sender).Channel
                    await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);

                    _logger.LogInformation("Payment Worker - Executed Payment to OrderId {orderId} - running at: {time}", order.Id, DateTimeOffset.Now);
                }

            };

            await channel.BasicConsumeAsync("orders", autoAck: false, consumer: consumer);


            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        private async Task<IChannel> CreateConnection(CancellationToken stoppingToken)
        {
            IConnection connection = null!;
            IChannel channel = null!;

            var factory = new ConnectionFactory
            {
                HostName = "rabbitmq",
                UserName = "guest",
                Password = "guest"
            };

            var retries = 10;

            for (int i = 0; i < retries; i++)
            {
                try
                {
                    connection = await factory.CreateConnectionAsync();
                    channel = await connection.CreateChannelAsync();
                    break;
                }
                catch
                {
                    _logger.LogWarning("Retry 5s...");
                    await Task.Delay(5000, stoppingToken);
                }
            }

            return channel;
        }
    }
}
