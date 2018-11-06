using Microsoft.Extensions.DependencyInjection;
using RiverLinkReporter.service;
using RiverLinkReporter.Service;

namespace RiverLinkReporter.api
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterMyServices(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddSingleton<IEmailService, EmailService>();
            services.AddTransient<IVehicleService, VehicleService>();
            services.AddTransient<ITransponderService, TransponderService>();
            services.AddTransient<ITransactionService, TransactionService>();

            // Add all other services here.
            return services;
        }
    }
}
