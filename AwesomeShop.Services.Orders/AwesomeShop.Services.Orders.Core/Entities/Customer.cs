using System;

namespace AwesomeShop.Services.Orders.Core.Entities
{
    public class Customer : IEntityBase
    {
        public Customer(Guid id, string fullName, string email)
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