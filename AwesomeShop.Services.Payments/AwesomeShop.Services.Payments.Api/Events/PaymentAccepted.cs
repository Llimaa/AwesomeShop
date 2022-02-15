using System;

namespace AwesomeShop.Services.Payments.Api.Events
{
    public class PaymentAccepted
    {
        public PaymentAccepted(Guid id, string fullName, string email)
        {
            this.Id = id;
            this.FullName = fullName;
            this.Email = email;

        }
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}