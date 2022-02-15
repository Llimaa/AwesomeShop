using System;
using MediatR;

namespace AwesomeShop.Services.Customers.Application.Commands
{
    public class AddCustomer : IRequest<Guid>
    {
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
    }
}