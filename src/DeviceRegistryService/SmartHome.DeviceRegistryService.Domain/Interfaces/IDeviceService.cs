using SmartHome.DeviceRegistryService.Domain.Models;

namespace SmartHome.DeviceRegistryService.Domain.Interfaces
{
    public interface IDeviceService
    {
        Task<IEnumerable<DeviceDto>> GetAllDevicesAsync();
        Task<DeviceDto?> GetDeviceByIdAsync(int id);
        Task<DeviceDto> CreateDeviceAsync(CreateDeviceRequest request);
        Task DeleteDeviceAsync(int id);
        Task<DeviceDto> RegisterDeviceAsync(int id);
    }
}