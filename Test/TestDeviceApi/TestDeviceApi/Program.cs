var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
var random = new Random();


app.MapPost("/api/devices/{deviceId}/command", async (int deviceId, DeviceCommandRequest request) =>
{
    Console.WriteLine($"Received command for device {deviceId}: {request.Command} with value {request.Value}");
    return Results.Ok(new
    {
        success = true,
        message = $"Command {request.Command} executed for device {deviceId}"
    });
});

// GET /temperature?location={location} - Get temperature by location
app.MapGet("/temperature", (string location) =>
{
    return Results.Ok(new
    {
        value = Math.Round(18 + random.NextDouble() * 10, 1),
        unit = "celsius",
        timestamp = DateTime.UtcNow,
        location,
        status = "OK",
        sensor_id = $"sensor-{random.Next(100, 999)}",
        sensor_type = "digital",
        description = $"Temperature sensor at {location}"
    });
});

// GET /temperature/{sensorId} - Get temperature by sensor ID
app.MapGet("/temperature/{sensorId}", (string sensorId) =>
{
    return Results.Ok(new
    {
        value = Math.Round(18 + random.NextDouble() * 10, 1),
        unit = "celsius",
        timestamp = DateTime.UtcNow,
        location = $"location-{random.Next(1, 5)}",
        status = "OK",
        sensor_id = sensorId,
        sensor_type = "digital",
        description = $"Temperature sensor {sensorId}"
    });
});


// Health check endpoint
app.MapGet("/health", () => Results.Ok(new {status= "healthy" }));


app.Run("http://*:5000");

public record DeviceCommandRequest(string Command, float Value);