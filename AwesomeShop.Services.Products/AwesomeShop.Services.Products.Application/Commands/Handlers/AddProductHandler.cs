using System;
using System.Threading;
using System.Threading.Tasks;
using AwesomeShop.Services.Products.Core.Entities;
using AwesomeShop.Services.Products.Core.Repositories;
using MediatR;

namespace AwesomeShop.Services.Products.Application.Commands.Handlers
{
    public class AddProductHandler : IRequestHandler<AddProduct, Guid>
    {
        private readonly IProductRepository _repository;
        public AddProductHandler(IProductRepository repository)
        {
            _repository = repository;
        }
        
        public async Task<Guid> Handle(AddProduct request, CancellationToken cancellationToken)
        {
            var product = request.ToEntity();

            await _repository.AddAsync(product);

            return product.Id;
        }
    }
}