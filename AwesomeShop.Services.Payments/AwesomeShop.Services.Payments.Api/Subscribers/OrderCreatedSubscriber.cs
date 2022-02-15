using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AwesomeShop.Services.Payments.Api.Core.Entities;
using AwesomeShop.Services.Payments.Api.Events;
using AwesomeShop.Services.Payments.Api.Infrastructure.PaymentGateway;
using AwesomeShop.Services.Payments.Api.Infrastructure.PaymentGateway.Dtos;
using AwesomeShop.Services.Payments.Api.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AwesomeShop.Services.Payments.Api.Subscribers
{
    public class OrderCreatedSubscriber : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private const string Queue = "payment-service/order-created";
        private const string Exchange = "payment-service";
        public OrderCreatedSubscriber(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

             var connectionFactory = new ConnectionFactory {
                HostName = "localhost"
            };

            _connection = connectionFactory.CreateConnection("payment-service-order-created-consumer"); 

            _channel = _connection.CreateModel();
            
            _channel.ExchangeDeclare(Exchange, "topic", true);
            _channel.QueueDeclare(Queue, false, false, false, null);
            _channel.QueueBind(Queue, Exchange, Queue);

            _channel.QueueBind(Queue, "order-service", "order-created");
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (sender, eventArgs) => {
                var contentArray = eventArgs.Body.ToArray();
                var contentString = Encoding.UTF8.GetString(contentArray);
                var message = JsonConvert.DeserializeObject<OrderCreated>(contentString);

                Console.WriteLine($"Message OrderCreated received with Id {message.Id}");

                var result = await ProcessPayment(message);

                if (result) {
                    _channel.BasicAck(eventArgs.DeliveryTag, false);

                    var paymentAccepted = new PaymentAccepted(message.Id, message.FullName, message.Email);
                    var payload = JsonConvert.SerializeObject(paymentAccepted);
                    var byteArray = Encoding.UTF8.GetBytes(payload);

                    Console.WriteLine("PaymentAccepted Published");
                    
                    _channel.BasicPublish(Exchange, "payment-accepted", null, byteArray);
                }
            };

            _channel.BasicConsume(Queue, false, consumer);

            return Task.CompletedTask;
        }

        private async Task<bool> ProcessPayment(OrderCreated orderCreated) {
            using (var scope = _serviceProvider.CreateScope()) {
                var paymentService = scope.ServiceProvider.GetService<IPaymentGatewayService>();

                var result = await paymentService
                    .Process(new CreditCardInfo(
                        orderCreated.PaymentInfo.CardNumber,
                        orderCreated.PaymentInfo.FullName,
                        orderCreated.PaymentInfo.ExpirationDate,
                        orderCreated.PaymentInfo.Cvv));

                var invoiceRepository = scope.ServiceProvider.GetService<IInvoiceRepository>();

                await invoiceRepository.AddAsync(new Invoice(orderCreated.TotalPrice, orderCreated.Id, orderCreated.PaymentInfo.CardNumber));
                
                return result;
            }
        }
    }
    
    public class OrderCreated {
        public Guid Id { get; set; }
        public decimal TotalPrice { get; set; }
        public PaymentInfo PaymentInfo { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }

    public class PaymentInfo {
        public string CardNumber { get; set; }
        public string FullName { get; set; }
        public string ExpirationDate { get; set; }
        public string Cvv { get; set; }
    }
}