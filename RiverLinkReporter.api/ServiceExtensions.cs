using Microsoft.Extensions.DependencyInjection;
using RiverLinkReporter.service;
using RiverLinkReporter.service.Data;

namespace RiverLinkReporter.api
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterMyServices(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddSingleton<IEmailService, EmailService>();
            services.AddTransient<IRepository, EFRepository<ApplicationDbContext>>();

            // Add all other services here.
            return services;
        }
    }
}
