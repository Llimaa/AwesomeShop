

using RabbitMQ.Client;

namespace AwesomeShop.Services.Customers.Infrastructure.MessageBus
{
    public class ProducerConnection
    {
        public ProducerConnection(IConnection connection)
        {
            Connection = connection;

        }

        public IConnection Connection { get; private set; }
    }
}