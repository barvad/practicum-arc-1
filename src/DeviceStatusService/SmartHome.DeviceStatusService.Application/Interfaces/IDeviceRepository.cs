using SmartHome.DeviceStatusService.Domain.Entities;

namespace SmartHome.DeviceStatusService.Application.Interfaces
{
    public interface IDeviceRepository
    {
        Task<Device> GetByIdAsync(int id);
        Task UpdateStatusAsync(int id, string status, float value);
        Task<IEnumerable<Device>> GetAllAsync();
    }
}

