namespace SmartHome.DeviceRegistryService.Domain.Models
{
    public class DeviceDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public float Value { get; set; }
        public string Unit { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime LastUpdated { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateDeviceRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
    }

    public class UpdateDeviceRequest
    {
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? Location { get; set; }
        public string? Unit { get; set; }
        public string? Status { get; set; }
        public float? Value { get; set; }
    }
}