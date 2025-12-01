using Microsoft.AspNetCore.Mvc;
using SmartHome.DeviceStatusService.Domain.Interfaces;
using SmartHome.DeviceStatusService.Domain.Models;

namespace SmartHome.DeviceStatusService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeviceStatusController : ControllerBase
    {
        private readonly IDeviceStatusService _deviceStatusService;
        private readonly ILogger<DeviceStatusController> _logger;

        public DeviceStatusController(
            IDeviceStatusService deviceStatusService,
            ILogger<DeviceStatusController> logger)
        {
            _deviceStatusService = deviceStatusService;
            _logger = logger;
        }

        [HttpGet("{deviceId}")]
        public async Task<ActionResult<DeviceStatus>> GetStatus(int deviceId)
        {
            try
            {
                var status = await _deviceStatusService.UpdateAndGetDeviceStatus(deviceId);
                return Ok(status);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting status for device {DeviceId}", deviceId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{deviceId}")]
        public async Task<IActionResult> UpdateStatus(int deviceId, [FromBody] UpdateStatusRequest request)
        {
            try
            {
                await _deviceStatusService.UpdateDeviceStatus(deviceId, request.Status, request.Value);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating status for device {DeviceId}", deviceId);
                return StatusCode(500, "Internal server error");
            }
        }
    }

    public class UpdateStatusRequest
    {
        public string Status { get; set; } = string.Empty;
        public float Value { get; set; }
    }
}

