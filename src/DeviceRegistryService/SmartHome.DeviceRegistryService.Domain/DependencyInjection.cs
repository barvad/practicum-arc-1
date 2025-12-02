using Microsoft.Extensions.DependencyInjection;
using SmartHome.DeviceRegistryService.Domain.Interfaces;
using SmartHome.DeviceRegistryService.Domain.Services;

namespace SmartHome.DeviceRegistryService.Domain
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IDeviceService, DeviceService>();
            return services;
        }
    }
}