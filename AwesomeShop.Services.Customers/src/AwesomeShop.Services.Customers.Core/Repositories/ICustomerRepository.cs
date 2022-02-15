using System;
using System.Threading.Tasks;
using AwesomeShop.Services.Customers.Core.Entities;

namespace AwesomeShop.Services.Customers.Core.Repositories
{
    public interface ICustomerRepository
    {
         Task<Customer> GetByIdAsync(Guid id);
         Task AddAsync(Customer customer);
         Task UpdateAsync(Customer customer);
    }
}