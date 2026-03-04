using eCommerce.SharedLibrary.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.SharedLibrary.DI
{
    public static class SharedServiceContainer
    {
        public static IServiceCollection AddSharedService<TContext>
            (this IServiceCollection service, IConfiguration config, string fileName) where TContext : DbContext
        {

            // Add Generic shared context
            service.AddDbContext<TContext>(option => option.UseSqlServer(
                config.GetConnectionString("eCommerceConnection"), sqlserverOption => 
                sqlserverOption.EnableRetryOnFailure())
            );

            // configure serilog
            Log.Logger = new LoggerConfiguration().
                MinimumLevel.Information().
                WriteTo.Debug().
                WriteTo.Console().
                WriteTo.File(path: $"Logs/{fileName}-.text",
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                outputTemplate: "{Timestamp: yyyy-MM-DD HH:mm:ss.fff zzz [{Level : u3}] {message : lj} {NewLine}{Exception}",
                rollingInterval: RollingInterval.Day).
                CreateLogger();


            // Add JWT authentication scheme
            JWTAuhenticationScheme.AddJWTAuthenticationScheme(service, config);
            return service;
        }

        public static IApplicationBuilder UserSharedPolicies(this IApplicationBuilder app)
        {
            //use global exception
            app.UseMiddleware<GlobalException>();

            //Register middleware to block all unauthorized users
            app.UseMiddleware<ListenToOnlyAPIGateway>();

            return app;
        }
    }
}
