using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AwesomeShop.Services.Notifications.Api.Infrastructure.Persistence.Repositories;
using AwesomeShop.Services.Notifications.Api.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AwesomeShop.Services.Notifications.Api.Subscribers
{
    public class CustomerCreatedSubscriber : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private const string Queue = "notification-service/customer-created";
        private const string Exchange = "notification-service";
        public CustomerCreatedSubscriber(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            var connectionFactory = new ConnectionFactory {
                HostName = "localhost"
            };

            _connection = connectionFactory.CreateConnection("notifications-service-customer-created-consumer"); 

            _channel = _connection.CreateModel();
            
            _channel.ExchangeDeclare(Exchange, "topic", true);
            _channel.QueueDeclare(Queue, false, false, false, null);
            _channel.QueueBind(Queue, Exchange, Queue);

            _channel.QueueBind(Queue, "customer-service", "customer-created");
        }
        
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (sender, eventArgs) => {
                var contentArray = eventArgs.Body.ToArray();
                var contentString = Encoding.UTF8.GetString(contentArray);
                var message = JsonConvert.DeserializeObject<CustomerCreated>(contentString);

                Console.WriteLine($"Message CustomerCreated received with Id {message.Id}");

                await SendEmail(message);

                _channel.BasicAck(eventArgs.DeliveryTag, false);
            };

            _channel.BasicConsume(Queue, false, consumer);
                        
            return Task.CompletedTask;
        }

        private async Task<bool> SendEmail(CustomerCreated customer) {
            using (var scope = _serviceProvider.CreateScope()) {
                var emailService = scope.ServiceProvider.GetService<INotificationService>();
                var mailRepository = scope.ServiceProvider.GetService<IMailRepository>();

               // var template = await mailRepository.GetTemplate("CustomerCreated");

                var subject = string.Format("marcoslimadefatima@gmail.com", customer.FullName);
                var content = string.Format("marcoslimadefatima@gmail.com", customer.FullName);

                await emailService.SendAsync(subject, content, customer.Email, customer.FullName);

                return true;
            }
        }
    }

    public class CustomerCreated {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}