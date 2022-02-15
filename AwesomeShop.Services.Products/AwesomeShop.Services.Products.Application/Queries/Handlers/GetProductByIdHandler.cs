using System.Threading;
using System.Threading.Tasks;
using AwesomeShop.Services.Products.Application.Dtos.ViewModels;
using AwesomeShop.Services.Products.Core.Repositories;
using MediatR;

namespace AwesomeShop.Services.Products.Application.Queries.Handlers
{
    public class GetProductByIdHandler : IRequestHandler<GetProductById, ProductDetailsViewModel>
    {
        private readonly IProductRepository _repository;
        public GetProductByIdHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductDetailsViewModel> Handle(GetProductById request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(request.Id);

            var productDetails = ProductDetailsViewModel.FromEntity(product);

            return productDetails;
        }
    }
}