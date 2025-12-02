using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartHome.DeviceRegistryService.Domain.Interfaces;
using SmartHome.DeviceRegistryService.Domain.Services;
using SmartHome.DeviceRegistryService.Infrastructure.Data;

namespace SmartHome.DeviceRegistryService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDeviceRepository, DeviceRepository>();
            services.AddScoped<IDatabaseConnectionFactory>(_ => 
                new DatabaseConnectionFactory(configuration.GetConnectionString("DefaultConnection")));
            return services;
        }
    }
}