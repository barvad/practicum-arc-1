using Microsoft.Extensions.DependencyInjection;
using SmartHome.DeviceStatusService.Application.Interfaces;
using SmartHome.DeviceStatusService.Application.Services;

namespace SmartHome.DeviceStatusService.Application
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

