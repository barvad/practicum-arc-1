using Microsoft.Extensions.Logging;
using SmartHome.DeviceRegistryService.Domain.Entities;
using SmartHome.DeviceRegistryService.Domain.Interfaces;
using SmartHome.DeviceRegistryService.Domain.Models;

namespace SmartHome.DeviceRegistryService.Domain.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly ILogger<DeviceService> _logger;

        public DeviceService(
            IDeviceRepository deviceRepository,
            ILogger<DeviceService> logger)
        {
            _deviceRepository = deviceRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<DeviceDto>> GetAllDevicesAsync()
        {
            var devices = await _deviceRepository.GetAllAsync();
            return devices.Select(MapToDto);
        }

        public async Task<DeviceDto?> GetDeviceByIdAsync(int id)
        {
            var device = await _deviceRepository.GetByIdAsync(id);
            return device != null ? MapToDto(device) : null;
        }

        public async Task<DeviceDto> CreateDeviceAsync(CreateDeviceRequest request)
        {
            var device = new Device
            {
                Name = request.Name,
                Type = request.Type,
                Location = request.Location,
                Unit = request.Unit,
                Status = "inactive",
                Value = 0,
                CreatedAt = DateTime.UtcNow,
                LastUpdated = DateTime.UtcNow
            };

            var createdDevice = await _deviceRepository.CreateAsync(device);
            _logger.LogInformation("Created new device: {DeviceName} (ID: {DeviceId})", createdDevice.Name, createdDevice.Id);

            return MapToDto(createdDevice);
        }


        public async Task DeleteDeviceAsync(int id)
        {
            if (!await _deviceRepository.ExistsAsync(id))
                throw new ArgumentException($"Device with ID {id} not found");

            await _deviceRepository.DeleteAsync(id);
            _logger.LogInformation("Deleted device with ID: {DeviceId}", id);
        }

        public async Task<DeviceDto> RegisterDeviceAsync(int id)
        {
            var device = await _deviceRepository.GetByIdAsync(id);
            if (device == null)
                throw new ArgumentException($"Device with ID {id} not found");

            device.Status = "active";
            device.LastUpdated = DateTime.UtcNow;

            await _deviceRepository.UpdateAsync(device);

            
            _logger.LogInformation("Registered device: {DeviceName} (ID: {DeviceId})", device.Name, device.Id);

            return MapToDto(device);
        }

        private static DeviceDto MapToDto(Device device)
        {
            return new DeviceDto
            {
                Id = device.Id,
                Name = device.Name,
                Type = device.Type,
                Location = device.Location,
                Value = device.Value,
                Unit = device.Unit,
                Status = device.Status,
                LastUpdated = device.LastUpdated,
                CreatedAt = device.CreatedAt
            };
        }
    }
}