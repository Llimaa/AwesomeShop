using System;
using System.Threading.Tasks;
using AwesomeShop.Services.Customers.Core.Entities;
using AwesomeShop.Services.Customers.Core.Repositories;

namespace AwesomeShop.Services.Customers.Infrastructure.Persistence.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IMongoRepository<Customer> _mongoRepository;

        public CustomerRepository(IMongoRepository<Customer> mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }
        
        public async Task AddAsync(Customer customer)
        {
            await _mongoRepository.AddAsync(customer);
        }

        public async Task<Customer> GetByIdAsync(Guid id)
        {
            return await _mongoRepository.GetAsync(id);
        }

        public async Task UpdateAsync(Customer customer)
        {
            await _mongoRepository.UpdateAsync(customer);
        }
    }
}