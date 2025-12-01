using SmartHome.DeviceStatusService.Application.Models;

namespace SmartHome.DeviceStatusService.Application.Interfaces
{
    public interface IDeviceStatusService
    {
        Task ProcessDeviceCommand(DeviceCommand command);
        Task UpdateDeviceStatus(int deviceId, string status, float value);
        Task<DeviceStatus> GetDeviceStatus(int deviceId);
    }
}

