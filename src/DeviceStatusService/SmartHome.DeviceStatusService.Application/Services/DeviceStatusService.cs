using SmartHome.DeviceStatusService.Application.Interfaces;
using SmartHome.DeviceStatusService.Application.Models;
using Microsoft.Extensions.Logging;

namespace SmartHome.DeviceStatusService.Application.Services
{
    public class DeviceStatusService : IDeviceStatusService
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly IDeviceApiClient _deviceApiClient;
        private readonly ILogger<DeviceStatusService> _logger;

        public DeviceStatusService(
            IDeviceRepository deviceRepository,
            IDeviceApiClient deviceApiClient,
            ILogger<DeviceStatusService> logger)
        {
            _deviceRepository = deviceRepository;
            _deviceApiClient = deviceApiClient;
            _logger = logger;
        }

        public async Task ProcessDeviceCommand(DeviceCommand command)
        {
            try
            {
                _logger.LogInformation("Processing command for device {DeviceId}: {CommandType}", 
                    command.DeviceId, command.CommandType);

                await _deviceRepository.UpdateStatusAsync(command.DeviceId, command.Status, command.Value);

                var device = await _deviceRepository.GetByIdAsync(command.DeviceId);
                if (device != null)
                {
                    var apiRequest = new DeviceCommandRequest
                    {
                        Command = command.CommandType,
                        Value = command.Value
                    };

                    await _deviceApiClient.SendCommandAsync(command.DeviceId, apiRequest);
                    _logger.LogInformation("Successfully processed command for device {DeviceId}", command.DeviceId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing command for device {DeviceId}", command.DeviceId);
                throw;
            }
        }

        public async Task UpdateDeviceStatus(int deviceId, string status, float value)
        {
            await _deviceRepository.UpdateStatusAsync(deviceId, status, value);
        }

        public async Task<DeviceStatus> GetDeviceStatus(int deviceId)
        {
            var device = await _deviceRepository.GetByIdAsync(deviceId);
            
            if (device == null)
                throw new ArgumentException($"Device with ID {deviceId} not found");

            return new DeviceStatus
            {
                DeviceId = device.Id,
                Status = device.Status,
                Value = device.Value,
                LastUpdated = device.LastUpdated
            };
        }
    }
}

