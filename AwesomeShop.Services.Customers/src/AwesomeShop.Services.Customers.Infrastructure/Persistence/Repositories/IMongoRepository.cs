using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AwesomeShop.Services.Customers.Infrastructure.Persistence.Repositories
{
    public interface IMongoRepository<T>
    {
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task<T> GetAsync(Guid id);
        Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate);
    }
}