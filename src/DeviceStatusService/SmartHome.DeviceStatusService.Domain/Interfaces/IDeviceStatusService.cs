using SmartHome.DeviceStatusService.Domain.Models;

namespace SmartHome.DeviceStatusService.Domain.Interfaces
{
    public interface IDeviceStatusService
    {
        Task ProcessDeviceCommand(DeviceCommand command);
        Task UpdateDeviceStatus(int deviceId, string status, float value);
        Task<DeviceStatus> UpdateAndGetDeviceStatus(int deviceId);

        Task UpdateDevicesStatuses();
    }
}

