using System.Threading.Tasks;

namespace AwesomeShop.Services.Orders.Infrastructure.CacheStorage
{
    public interface ICacheService
    {
        Task<T> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T data);
    }
}