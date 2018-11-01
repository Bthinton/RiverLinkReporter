using Microsoft.Extensions.DependencyInjection;
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

            // Add all other services here.
            return services;
        }
    }
}
