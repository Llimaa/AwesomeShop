using System;
using System.Threading.Tasks;

namespace AwesomeShop.Services.Orders.Infrastructure.ServiceDiscovery
{
    public interface IServiceDiscoveryService
    {
        Task<Uri> GetServiceUri(string serviceName, string requestUrl);
    }
}