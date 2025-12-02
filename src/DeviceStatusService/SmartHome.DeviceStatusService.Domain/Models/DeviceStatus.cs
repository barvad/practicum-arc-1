namespace SmartHome.DeviceStatusService.Domain.Models
{
    public class DeviceStatus
    {
        public int DeviceId { get; set; }
        public string Status { get; set; } = string.Empty;
        public double Value { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}

