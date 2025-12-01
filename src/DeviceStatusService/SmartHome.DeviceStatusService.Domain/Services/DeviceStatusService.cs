using Microsoft.Extensions.Logging;
using SmartHome.DeviceStatusService.Domain.Interfaces;
using SmartHome.DeviceStatusService.Domain.Models;

namespace SmartHome.DeviceStatusService.Domain.Services;

public class DeviceStatusService : IDeviceStatusService
{
    private readonly IDeviceApiClient _deviceApiClient;
    private readonly IDeviceRepository _deviceRepository;
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

    public async Task<DeviceStatus> UpdateAndGetDeviceStatus(int deviceId)
    {
        var device = await _deviceRepository.GetByIdAsync(deviceId);

        if (device == null)
            throw new ArgumentException($"Device with ID {deviceId} not found");
        try
        {
            var temperature = await _deviceApiClient.GetTemperatureById(deviceId);
            await _deviceRepository.UpdateStatusAsync(deviceId, temperature.Status, temperature.Value);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error update and get device status");
            throw;
        }

        return new DeviceStatus
        {
            DeviceId = device.Id,
            Status = device.Status,
            Value = device.Value,
            LastUpdated = device.LastUpdated
        };
    }

    public async Task UpdateDevicesStatuses()
    {
        var devices = await _deviceRepository.GetAllAsync();
        foreach (var device in devices)
            try
            {
                var temperature = await _deviceApiClient.GetTemperatureById(device.Id);
                await _deviceRepository.UpdateStatusAsync(device.Id, temperature.Status, temperature.Value);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error update and get device status id {0}", device.Id);
                throw;
            }
    }
}