using System;
using AwesomeShop.Services.Customers.Application.View_Models;
using MediatR;

namespace AwesomeShop.Services.Customers.Application.Queries
{
    public class GetCustomerById : IRequest<GetCustomerByIdViewModel>
    {
        public GetCustomerById(Guid id)
        {
            Id = id;
        }
        
        public Guid Id { get; private set; }
    }
}