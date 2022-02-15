using AwesomeShop.Services.Products.Core.Repositories;
using AwesomeShop.Services.Products.Infrastructure.Persistence;
using AwesomeShop.Services.Products.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AwesomeShop.Services.Products.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddMongo(this IServiceCollection services) {
            services.AddSingleton(sp => {
                var configuration = sp.GetService<IConfiguration>();

                var options = new MongoDbOptions();
                configuration.GetSection("Mongo").Bind(options);

                return options;
            });

            services.AddSingleton(sp => {
                var options = sp.GetService<MongoDbOptions>();

                return new MongoClient(options.ConnectionString);
            });

            services.AddTransient(sp => {
                BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;

                var options = sp.GetService<MongoDbOptions>();
                var mongoClient = sp.GetService<MongoClient>();
                
                return mongoClient.GetDatabase(options.Database);
            });
            
            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services) {
            services.AddScoped<IProductRepository, ProductRepository>();

            return services;
        }
    }
}