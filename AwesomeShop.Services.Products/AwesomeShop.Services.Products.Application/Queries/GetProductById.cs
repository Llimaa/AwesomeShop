using System;
using AwesomeShop.Services.Products.Application.Dtos.ViewModels;
using MediatR;

namespace AwesomeShop.Services.Products.Application.Queries
{
    public class GetProductById : IRequest<ProductDetailsViewModel>
    {
        public GetProductById(Guid id)
        {
            this.Id = id;

        }
        public Guid Id { get; private set; }
    }
}