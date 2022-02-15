using System;

namespace AwesomeShop.Services.Products.Core.Events
{
    public class ProductUpdated : IDomainEvent
    {
        public ProductUpdated(Guid id)
        {
            Id = id;
        }
        
        public Guid Id { get; private set; }
    }
}