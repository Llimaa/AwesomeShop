using System.Threading.Tasks;
using AwesomeShop.Services.Payments.Api.Core.Entities;
using MongoDB.Driver;

namespace AwesomeShop.Services.Payments.Api.Infrastructure.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly IMongoCollection<Invoice> _collection;
        public InvoiceRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<Invoice>("invoices");
        }

        public async Task AddAsync(Invoice invoice)
        {
            await _collection.InsertOneAsync(invoice);
        }
    }
}