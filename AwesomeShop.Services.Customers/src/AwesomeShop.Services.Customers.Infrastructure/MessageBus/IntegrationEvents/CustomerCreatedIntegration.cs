using System;

namespace AwesomeShop.Services.Customers.Infrastructure.MessageBus.IntegrationEvents
{
    public class CustomerCreatedIntegration : IEvent
    {
        public CustomerCreatedIntegration(Guid id, string fullName, string email)
        {
            Id = id;
            FullName = fullName;
            Email = email;
        }
        
        public Guid Id { get; private set; }
        public string FullName { get; private set; }
        public string Email { get; private set; }
    }
}