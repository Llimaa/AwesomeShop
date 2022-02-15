using System;
using MediatR;

namespace AwesomeShop.Services.Products.Application.Commands
{
    public class UpdateProduct : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public CategoryDto Category { get; set; }
    }
}