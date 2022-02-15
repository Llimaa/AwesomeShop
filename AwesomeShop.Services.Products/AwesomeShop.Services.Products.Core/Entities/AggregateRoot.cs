using System;
using System.Collections.Generic;
using AwesomeShop.Services.Products.Core.Events;

namespace AwesomeShop.Services.Products.Core.Entities
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