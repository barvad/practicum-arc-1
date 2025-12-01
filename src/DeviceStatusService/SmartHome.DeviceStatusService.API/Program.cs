using Coravel;
using SmartHome.DeviceStatusService.API.Kafka;
using SmartHome.DeviceStatusService.Domain;
using SmartHome.DeviceStatusService.Domain.Interfaces;
using SmartHome.DeviceStatusService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("Kafka"));

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register application and infrastructure layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Register Kafka consumer as hosted service
//builder.Services.AddHostedService<DeviceCommandConsumerService>();

// Add scheduler
builder.Services.AddScheduler();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();


app.Services.UseScheduler(s =>
{
    s.ScheduleAsync(async () =>
    {
        try
        {
            using var scope = app.Services.CreateScope();
            var deviceStatusService = scope.ServiceProvider.GetRequiredService<IDeviceStatusService>();
            await deviceStatusService.UpdateDevicesStatuses();
        }
        catch (Exception e)
        {
            app.Logger.LogError(e, "Error while running job.");
        }
    }).Cron("* * * * *");
});
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();