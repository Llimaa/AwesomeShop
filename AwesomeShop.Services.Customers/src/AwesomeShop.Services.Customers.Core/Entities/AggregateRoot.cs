using System;
using System.Collections.Generic;
using System.Linq;
using AwesomeShop.Services.Customers.Core.Events;

namespace AwesomeShop.Services.Customers.Core.Entities
{
    public abstract class AggregateRoot : IEntityBase
    {
        private readonly List<IDomainEvent> _events = new List<IDomainEvent>();
        public Guid Id { get; protected set; }
        public IEnumerable<IDomainEvent> Events => _events;

        protected void AddEvent(IDomainEvent @event) {
            _events.Add(@event);
        }
    }
}