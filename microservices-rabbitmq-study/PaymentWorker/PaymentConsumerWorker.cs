using System.Text;
using System.Threading.Channels;
using Newtonsoft.Json;
using PaymentConsumer.Model;
using PaymentConsumer.Services.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace PaymentWorker
{
    public class PaymentConsumerWorker : BackgroundService
    {
        private readonly ILogger<PaymentConsumerWorker> _logger;
        private readonly IPaymentService _paymentService;

        public PaymentConsumerWorker(ILogger<PaymentConsumerWorker> logger, IPaymentService paymentService)
        {
            _logger = logger;
            _paymentService = paymentService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Payment Worker running at: {time}", DateTimeOffset.Now);

            IChannel channel = await CreateConnectionAsync(stoppingToken);

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
                    // publish payment approved message
                    await PaymentSenderAsync(order, stoppingToken);

                    // here channel could also be accessed as ((AsyncEventingBasicConsumer)sender).Channel
                    await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);

                    _logger.LogInformation("Payment Worker - Executed Payment to OrderId {orderId} - running at: {time}", order.Id, DateTimeOffset.Now);
                }

            };

            await channel.BasicConsumeAsync("orders", autoAck: false, consumer: consumer);


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

        private async Task PaymentSenderAsync(Order order, CancellationToken stoppingToken)
        {
            string queue = "approved_payments";

            IChannel channel = await CreateConnectionAsync(stoppingToken);

            await channel.QueueDeclareAsync(
               queue: queue,
               durable: true,
               exclusive: false,
               autoDelete: false,
               arguments: null);

            string orderJson = JsonConvert.SerializeObject(order);

            var body = Encoding.UTF8.GetBytes(orderJson);

            await channel.BasicPublishAsync(exchange: string.Empty, routingKey: queue, body: body);

            _logger.LogInformation("Payment Worker - Sended message in {queue} queue - running at: {time}", queue, DateTimeOffset.Now);

        }
    }
}
