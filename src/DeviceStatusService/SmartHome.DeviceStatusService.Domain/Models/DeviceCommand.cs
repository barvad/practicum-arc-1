namespace SmartHome.DeviceStatusService.Domain.Models
{
    public class DeviceCommand
    {
        public int DeviceId { get; set; }
        public string CommandType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public float Value { get; set; }
        public DateTime Timestamp { get; set; }
        public string CorrelationId { get; set; } = string.Empty;
    }
}

