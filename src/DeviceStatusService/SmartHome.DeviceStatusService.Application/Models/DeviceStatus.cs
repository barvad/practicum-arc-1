namespace SmartHome.DeviceStatusService.Application.Models
{
    public class DeviceStatus
    {
        public int DeviceId { get; set; }
        public string Status { get; set; } = string.Empty;
        public float Value { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}

