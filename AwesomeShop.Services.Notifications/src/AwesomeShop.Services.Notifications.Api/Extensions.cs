using AwesomeShop.Services.Notifications.Api.Infrastructure.Persistence;
using AwesomeShop.Services.Notifications.Api.Infrastructure.Persistence.Repositories;
using AwesomeShop.Services.Notifications.Api.Infrastructure.Services;
using AwesomeShop.Services.Notifications.Api.Subscribers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
using SendGrid.Extensions.DependencyInjection;

namespace AwesomeShop.Services.Notifications.Api
{
    public static class Extensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services) {
            services.AddScoped<IMailRepository,MailRepository>();

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

        public static IServiceCollection AddMailService(this IServiceCollection services, IConfiguration configuration) {
            var config = new MailConfig();

            configuration.GetSection("Notifications").Bind(config);

            services.AddSingleton<MailConfig>(m => config);
            
            services.AddSendGrid(sp => sp.ApiKey = config.SendGridApiKey);

            services.AddTransient<INotificationService, NotificationService>();

            return services;
        }

        public static IServiceCollection AddSubscribers(this IServiceCollection services) {
            services.AddHostedService<CustomerCreatedSubscriber>();
            services.AddHostedService<OrderCreatedSubscriber>();
            services.AddHostedService<PaymentAcceptedSubscriber>();

            return services;
        }
    }
}