using AwesomeShop.Services.Products.Application.Queries;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AwesomeShop.Services.Products.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddHandlers(this IServiceCollection services) {
            services.AddMediatR(typeof(GetAllProducts));
            
            return services;
        }
    }
}