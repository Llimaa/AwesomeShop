using System;
using System.Threading;
using System.Threading.Tasks;
using AwesomeShop.Services.Orders.Core.Repositories;
using AwesomeShop.Services.Orders.Infrastructure.MessageBus;
using AwesomeShop.Services.Orders.Infrastructure.ServiceDiscovery;
using MediatR;

namespace AwesomeShop.Services.Orders.Application.Commands.Handlers
{
    public class AddOrderHandler : IRequestHandler<AddOrder, Guid>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMessageBusClient _messageBus;
        private readonly IServiceDiscoveryService _serviceDiscovery;
        public AddOrderHandler(
            IOrderRepository orderRepository, 
            IMessageBusClient messageBus, 
            IServiceDiscoveryService serviceDiscovery)
        {
            _orderRepository = orderRepository;
            _messageBus = messageBus;
            _serviceDiscovery = serviceDiscovery;
        }

        public async Task<Guid> Handle(AddOrder request, CancellationToken cancellationToken)
        {
            var order = request.ToEntity();
            await _orderRepository.AddAsync(order);

            foreach (var @event in order.Events)
            {
                // OrderCreated = order-created
                var routingKey = @event.GetType().Name.ToDashCase();

                _messageBus.Publish(@event, routingKey, "order-service");
            }
            return order.Id;
        }
    }
}