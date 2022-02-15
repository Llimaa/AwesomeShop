using System;

namespace AwesomeShop.Services.Products.Core.Events
{
    public class ProductCreated : IDomainEvent
    {
        public ProductCreated(Guid id, string description)
        {
            Id = id;
            Description = description;
        }
        
        public Guid Id { get; private set; }
        public string Description { get; private set; }
    }
}