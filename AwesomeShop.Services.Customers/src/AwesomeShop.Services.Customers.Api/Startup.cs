using AwesomeShop.Services.Customers.Application.Commands;
using AwesomeShop.Services.Customers.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MediatR;
using AwesomeShop.Services.Customers.Application;

namespace AwesomeShop.Services.Customers.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMediatR(typeof(AddCustomer))
                .AddMongo()
                .AddRepositories()
                .AddRabbitMq()
                .AddSubscribers()
                .AddConsulConfig(Configuration);
            
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "AwesomeShop.Services.Customers.Api", Version = "v1"});
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AwesomeShop.Services.Customers.Api v1"));
            }

            app.UseRouting();
            app.UseConsul();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}