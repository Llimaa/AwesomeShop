
using Newtonsoft.Json;
using RabbitMQ.Client;
using Newtonsoft.Json.Serialization;
using System.Text;
using System;

namespace AwesomeShop.Services.Customers.Infrastructure.MessageBus
{
    public class RabbitMqClient : IMessageBusClient
    {
        private readonly IConnection _connection;
        public RabbitMqClient(ProducerConnection producerConnection)
        {
            _connection = producerConnection.Connection;
        }
        
        public void Publish(object message, string routingKey, string exchange)
        {
            if (routingKey.Contains("-integration")) {
                routingKey = routingKey.Replace("-integration", "");
            }

            var channel = _connection.CreateModel();

            var settings = new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var payload = JsonConvert.SerializeObject(message, settings);
            Console.WriteLine(payload);

            var body = Encoding.UTF8.GetBytes(payload);
            
            channel.ExchangeDeclare(exchange, "topic", true);

            channel.BasicPublish(exchange, routingKey, null, body);
        }
    }
}