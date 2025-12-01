using System.Text.Json.Serialization;
using Refit;

namespace SmartHome.DeviceStatusService.Domain.Interfaces;

public interface IDeviceApiClient
{
    [Post("/api/devices/{deviceId}/command")]
    Task SendCommandAsync(int deviceId, [Body] DeviceCommandRequest request);

    [Get("/temperature")]
    Task<TemperatureResponse> GetTemperatureByLocation([Query] string location);

    [Get("/temperature/{sensorId}")]
    Task<TemperatureResponse> GetTemperatureById(int sensorId);
}

public class DeviceCommandRequest
{
    public string Command { get; set; } = string.Empty;
    public float Value { get; set; }
}

public class TemperatureResponse
{
    [JsonPropertyName("value")] public double Value { get; set; }

    [JsonPropertyName("unit")] public string Unit { get; set; }

    [JsonPropertyName("timestamp")] public DateTime Timestamp { get; set; }

    [JsonPropertyName("location")] public string Location { get; set; }

    [JsonPropertyName("status")] public string Status { get; set; }

    [JsonPropertyName("sensor_id")] public string SensorId { get; set; }

    [JsonPropertyName("sensor_type")] public string SensorType { get; set; }

    [JsonPropertyName("description")] public string Description { get; set; }
}