using System;
using AwesomeShop.Services.Orders.Core.ValueObjects;

namespace AwesomeShop.Services.Orders.Core.Events
{
    public class OrderCreated : IDomainEvent
    {
        public OrderCreated(Guid id, decimal totalPrice, PaymentInfo paymentInfo, string fullName, string email)
        {
            Id = id;
            TotalPrice = totalPrice;
            PaymentInfo = paymentInfo;
            FullName = fullName;
            Email = email;
        }

        public Guid Id { get; }
        public decimal TotalPrice { get; }
        public PaymentInfo PaymentInfo { get; }
        public string FullName { get; }
        public string Email { get; }
    }
}