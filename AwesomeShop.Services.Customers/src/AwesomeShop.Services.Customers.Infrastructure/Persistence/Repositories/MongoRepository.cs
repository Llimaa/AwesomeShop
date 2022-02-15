using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AwesomeShop.Services.Customers.Core.Entities;
using MongoDB.Driver;

namespace AwesomeShop.Services.Customers.Infrastructure.Persistence.Repositories
{
    public class MongoRepository<T> : IMongoRepository<T> where T: IEntityBase
    {
        public MongoRepository(IMongoDatabase database, string collectionName)
        {
            Collection = database.GetCollection<T>(collectionName);
        }

        public IMongoCollection<T> Collection { get; private set; }

        public async Task AddAsync(T entity)
        {
            await Collection.InsertOneAsync(entity);
        }

        public async Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await Collection.Find(predicate).ToListAsync();
        }

        public async Task<T> GetAsync(Guid id)
        {
            return await Collection.Find(e => e.Id.Equals(id)).SingleOrDefaultAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            await Collection.ReplaceOneAsync(e => e.Id.Equals(entity.Id), entity);
        }
    }
}