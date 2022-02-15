using System.Collections.Generic;
using AwesomeShop.Services.Products.Application.Dtos.ViewModels;
using MediatR;

namespace AwesomeShop.Services.Products.Application.Queries
{
    public class GetAllProducts : IRequest<List<ProductViewModel>>
    {
        
    }
}