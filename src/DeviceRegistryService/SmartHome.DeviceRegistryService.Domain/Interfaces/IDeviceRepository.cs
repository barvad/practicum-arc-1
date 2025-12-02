using SmartHome.DeviceRegistryService.Domain.Entities;

namespace SmartHome.DeviceRegistryService.Domain.Interfaces
{
    public interface IDeviceRepository
    {
        Task<IEnumerable<Device>> GetAllAsync();
        Task<Device?> GetByIdAsync(int id);
        Task<Device> CreateAsync(Device device);
        Task UpdateAsync(Device device);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}