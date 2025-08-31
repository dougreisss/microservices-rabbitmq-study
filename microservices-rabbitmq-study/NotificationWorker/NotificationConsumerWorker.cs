using Newtonsoft.Json;
using System.Text;
using NotificationConsumer.Services.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using NotificationConsumer.Model;
using NotificationConsumer.Services;
using NotificationWorker.Model;

namespace NotificationWorker
{
    public class NotificationConsumerWorker : BackgroundService
    {
        private readonly ILogger<NotificationConsumerWorker> _logger;
        private readonly INotificationService _notificationService;
        public NotificationConsumerWorker(ILogger<NotificationConsumerWorker> logger, INotificationService notificationService)
        {
            _logger = logger;
            _notificationService = notificationService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Notification Worker running at: {time}", DateTimeOffset.Now);

            IChannel channel = await CreateConnectionAsync(stoppingToken);

            await channel.QueueDeclareAsync(queue: "approved_payments", durable: true, exclusive: false,
               autoDelete: false, arguments: null);

            await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);

            _logger.LogInformation("Notification Worker - Waiting for messages - running at: {time}", DateTimeOffset.Now);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                _logger.LogInformation("Notification Worker - Received {message}- running at: {time}", message, DateTimeOffset.Now);

                Order? order = JsonConvert.DeserializeObject<Order>(message);

                // send notification to client
                Notification notification = GetNotification(order);

                _notificationService.ExecuteNotification(notification);

                // here channel could also be accessed as ((AsyncEventingBasicConsumer)sender).Channel
                await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);

                _logger.LogInformation("Notification Worker - Executed Notification to OrderId {orderId} - running at: {time}", order.Id, DateTimeOffset.Now);
            };

            await channel.BasicConsumeAsync("approved_payments", autoAck: false, consumer: consumer);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        private async Task<IChannel> CreateConnectionAsync(CancellationToken stoppingToken)
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

        private Notification GetNotification(Order order)
        {
            // todo get notification data
            return new Notification
            {
                ClientName = "Douglas",
                ClientEmail = "dougreisrrs@gmail.com",
                ProductName = "Mouse",
                QuantityProduct = 3,
                UnitPrice = 100
            };
        }
    }
}
