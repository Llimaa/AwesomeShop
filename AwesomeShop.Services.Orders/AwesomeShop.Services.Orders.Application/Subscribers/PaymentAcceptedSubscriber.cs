using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AwesomeShop.Services.Orders.Core.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AwesomeShop.Services.Orders.Application.Subscribers
{
    public class PaymentAcceptedSubscriber : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private const string Queue = "order-service/payment-accepted";
        private const string Exchange = "order-service";
        private const string RoutingKey = "payment-accepted";

        public PaymentAcceptedSubscriber(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            var connectionFactory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            _connection = connectionFactory.CreateConnection("order-service-payment-accepted-subscriber");

            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(Exchange, "topic", true);
            _channel.QueueDeclare(Queue, false, false, false, null);
            _channel.QueueBind(Queue, "payment-service", RoutingKey);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (sender, eventArgs) =>
            {
                var byteArray = eventArgs.Body.ToArray();

                var connectionString = Encoding.UTF8.GetString(byteArray);
                var message = JsonConvert.DeserializeObject<PaymentAccepted>(connectionString);

                Console.WriteLine($"Message paymentAccept received with Id: {message.Id}");

                var result = await UpdateOrder(message);

                if (result)
                    _channel.BasicAck(eventArgs.DeliveryTag, false);
            };

            _channel.BasicConsume(Queue, false, consumer);

            return Task.CompletedTask;
        }

        private async Task<bool> UpdateOrder(PaymentAccepted paymentAccepted)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var orderRepository = scope.ServiceProvider.GetService<IOrderRepository>();

                var order = await orderRepository.GetByIdAsync(paymentAccepted.Id);
                order.SetAsCompleted();

                await orderRepository.UpdateAsync(order);
                return true;
            }
        }
    }

    public class PaymentAccepted
    {
        public Guid Id { get; set; }
        public string FullName { get; private set; }
        public string Email { get; private set; }
    }
}