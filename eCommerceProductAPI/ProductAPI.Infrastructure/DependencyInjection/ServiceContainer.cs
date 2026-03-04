using eCommerce.SharedLibrary.DI;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductAPI.App.Interfaces;
using ProductAPI.Infrastructure.Data;
using ProductAPI.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductAPI.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfraStructureServices(this IServiceCollection services, IConfiguration config)
        {
            // Register your infrastructure services here
            // e.g., services.AddScoped<IProductRepository, ProductRepository>();

            //Add database connection
            //Add authentication schemes

            SharedServiceContainer.AddSharedService<ProductDbContext>(services, config, config.GetValue<string>("MySerilog:FileName"));

            //Create dependency injections
            services.AddScoped<IProduct, ProductRepository>();

            return services;
        }

        public static IApplicationBuilder UseInfraStructurePolicy(this IApplicationBuilder app)
        {
            //Register middleware such as
            //Global exception: handle external errors
            // Listen to only API gateway: blocks all outsider calls
            SharedServiceContainer.UserSharedPolicies(app);

            return app;

        }
    }
}
