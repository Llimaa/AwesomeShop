using System;
using AwesomeShop.Services.Customers.Core.ValueObjects;

namespace AwesomeShop.Services.Customers.Core.Events
{
    public class CustomerUpdated : IDomainEvent
    {
        public CustomerUpdated(Guid id, string phoneNumber, Address address)
        {
            Id = id;
            Address = address;
        }

        public Guid Id { get; private set; }
        public Address Address { get; private set; }
    }
}