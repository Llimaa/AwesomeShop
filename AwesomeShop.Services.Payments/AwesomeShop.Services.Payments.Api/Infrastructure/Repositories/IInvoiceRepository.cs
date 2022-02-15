using System.Threading.Tasks;
using AwesomeShop.Services.Payments.Api.Core.Entities;

namespace AwesomeShop.Services.Payments.Api.Infrastructure.Repositories
{
    public interface IInvoiceRepository
    {
        Task AddAsync(Invoice invoice);
    }
}