using Refit;

namespace SmartHome.DeviceStatusService.Application.Interfaces
{
    public interface IDeviceApiClient
    {
        [Post("/api/devices/{deviceId}/command")]
        Task SendCommandAsync(int deviceId, [Body] DeviceCommandRequest request);
    }

    public class DeviceCommandRequest
    {
        public string Command { get; set; } = string.Empty;
        public float Value { get; set; }
    }
}

