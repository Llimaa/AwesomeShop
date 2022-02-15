using AwesomeShop.Services.Payments.Api.Infrastructure.PaymentGateway;
using AwesomeShop.Services.Payments.Api.Infrastructure.Repositories;
using AwesomeShop.Services.Payments.Api.Subscribers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AwesomeShop.Services.Payments.Api
{
    public static class Extensions
    {
        public static IServiceCollection AddPaymentGateway(this IServiceCollection services) {
            services.AddTransient<IPaymentGatewayService, MyPaymentGatewayService>();

            return services;
        }

        public static IServiceCollection AddSubscribers(this IServiceCollection services) {
            services.AddHostedService<OrderCreatedSubscriber>();
            
            return services;
        }

        public static IServiceCollection AddMongo(this IServiceCollection services) {
            services.AddSingleton(sp => {
                var configuration = sp.GetService<IConfiguration>();
                var options = new MongoDbOptions();

                configuration.GetSection("Mongo").Bind(options);

                return options;
            });

            services.AddSingleton<IMongoClient>(sp => {
                var options = sp.GetService<MongoDbOptions>();
                
                return new MongoClient(options.ConnectionString);
            });

            services.AddTransient(sp => {
                BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;
                
                var options = sp.GetService<MongoDbOptions>();
                var mongoClient = sp.GetService<IMongoClient>();

                return mongoClient.GetDatabase(options.Database);
            });

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services) {
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            
            return services;
        }
    }
}