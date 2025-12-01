using Microsoft.Extensions.DependencyInjection;
using SmartHome.DeviceStatusService.Domain.Interfaces;

namespace SmartHome.DeviceStatusService.Domain
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IDeviceStatusService, Services.DeviceStatusService>();
            return services;
        }
    }
}

