using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using SmartHome.DeviceStatusService.Domain.Interfaces;
using SmartHome.DeviceStatusService.Infrastructure.Data;


namespace SmartHome.DeviceStatusService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDeviceRepository, DeviceRepository>();
            services.AddScoped<IDatabaseConnectionFactory>(_ => 
                new DatabaseConnectionFactory(configuration.GetConnectionString("DefaultConnection")));

            services.AddRefitClient<IDeviceApiClient>()
                .ConfigureHttpClient(client => 
                {
                    client.BaseAddress = new Uri(configuration["DeviceApi:BaseUrl"] ?? "http://localhost:5002");
                });
            
            

            return services;
        }
    }
}

