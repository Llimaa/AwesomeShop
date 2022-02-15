using System;
using AwesomeShop.Services.Orders.Core.Repositories;
using AwesomeShop.Services.Orders.Infrastructure.CacheStorage;
using AwesomeShop.Services.Orders.Infrastructure.MessageBus;
using AwesomeShop.Services.Orders.Infrastructure.Persistence;
using AwesomeShop.Services.Orders.Infrastructure.Persistence.Repositories;
using AwesomeShop.Services.Orders.Infrastructure.ServiceDiscovery;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson;
using MongoDB.Driver;
using RabbitMQ.Client;

namespace AwesomeShop.Services.Orders.Infrastructure
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
            services.AddScoped<IOrderRepository, OrderRepository>();

            return services;
        }

        public static IServiceCollection AddMessageBus(this IServiceCollection services) {
            var connectionFactory = new ConnectionFactory {
                HostName = "localhost"
            };
            
            var connection = connectionFactory.CreateConnection("order-service-producer");

            services.AddSingleton(new ProducerConnection(connection));
            services.AddSingleton<IMessageBusClient, RabbitMqClient>();

            return services;
        }

        public static IServiceCollection AddConsulConfig(this IServiceCollection services, IConfiguration config) {
            services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig => {
                var address = config.GetValue<string>("Consul:Host");

                consulConfig.Address = new Uri(address);
            }));

            services.AddTransient<IServiceDiscoveryService, ConsulService>();
            
            return services;
        }

        public static IApplicationBuilder UseConsul(this IApplicationBuilder app) {
            var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
            var lifeTime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();

            var registration = new AgentServiceRegistration {
                ID = $"order-service-{Guid.NewGuid()}",
                Name = "OrderServices",
                Address = "localhost",
                Port = 5003
            };

            consulClient.Agent.ServiceDeregister(registration.ID).ConfigureAwait(true);
            consulClient.Agent.ServiceRegister(registration).ConfigureAwait(true);

            Console.WriteLine("Service registered in Consul");

            lifeTime.ApplicationStopping.Register(() => {
                consulClient.Agent.ServiceDeregister(registration.ID).ConfigureAwait(true);
                Console.WriteLine("Service Deregistered in Consul");
            });

            return app;
        }

        public static IServiceCollection AddRedisCache(this IServiceCollection services) {
            services.AddStackExchangeRedisCache(options => {
                options.InstanceName = "OrdersCache";
                options.Configuration = "localhost:6379";
            });

            services.AddTransient<ICacheService, CacheService>();
            
            return services;
        }
    }
}