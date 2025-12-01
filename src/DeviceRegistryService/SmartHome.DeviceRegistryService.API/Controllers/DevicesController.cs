using Microsoft.AspNetCore.Mvc;
using SmartHome.DeviceRegistryService.Domain.Interfaces;
using SmartHome.DeviceRegistryService.Domain.Models;

namespace SmartHome.DeviceRegistryService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DevicesController : ControllerBase
    {
        private readonly IDeviceService _deviceService;
        private readonly ILogger<DevicesController> _logger;

        public DevicesController(
            IDeviceService deviceService,
            ILogger<DevicesController> logger)
        {
            _deviceService = deviceService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeviceDto>>> GetAll()
        {
            try
            {
                var devices = await _deviceService.GetAllDevicesAsync();
                return Ok(devices);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all devices");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DeviceDto>> GetById(int id)
        {
            try
            {
                var device = await _deviceService.GetDeviceByIdAsync(id);
                
                if (device == null)
                    return NotFound($"Device with ID {id} not found");
                
                return Ok(device);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting device by ID: {DeviceId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<DeviceDto>> Create([FromBody] CreateDeviceRequest request)
        {
            try
            {
                var device = await _deviceService.CreateDeviceAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = device.Id }, device);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating device");
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _deviceService.DeleteDeviceAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting device: {DeviceId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("{id}/register")]
        public async Task<ActionResult<DeviceDto>> Register(int id)
        {
            try
            {
                var device = await _deviceService.RegisterDeviceAsync(id);
                return Ok(device);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering device: {DeviceId}", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}