using SmartHome.DeviceStatusService.Domain.Entities;

namespace SmartHome.DeviceStatusService.Domain.Interfaces
{
    public interface IDeviceRepository
    {
        Task<Device> GetByIdAsync(int id);
        Task UpdateStatusAsync(int id, string status, double value);
        Task<IEnumerable<Device>> GetAllAsync();
    }
}

