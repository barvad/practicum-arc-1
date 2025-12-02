using Dapper;
using SmartHome.DeviceStatusService.Domain.Entities;
using SmartHome.DeviceStatusService.Domain.Interfaces;
using SmartHome.DeviceStatusService.Infrastructure.Data;

namespace SmartHome.DeviceStatusService.Infrastructure.Data
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly IDatabaseConnectionFactory _connectionFactory;

        public DeviceRepository(IDatabaseConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Device> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            
            const string sql = @"
                SELECT id, name, type, location, value, unit, status, last_updated as LastUpdated, created_at as CreatedAt
                FROM sensors 
                WHERE id = @Id";

            return await connection.QuerySingleOrDefaultAsync<Device>(sql, new { Id = id });
        }

        public async Task UpdateStatusAsync(int id, string status, double value)
        {
            using var connection = _connectionFactory.CreateConnection();
            
            const string sql = @"
                UPDATE sensors 
                SET status = @Status, value = @Value, last_updated = NOW() 
                WHERE id = @Id";

            await connection.ExecuteAsync(sql, new { Id = id, Status = status, Value = value });
        }

        public async Task<IEnumerable<Device>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            
            const string sql = @"
                SELECT id, name, type, location, value, unit, status, last_updated as LastUpdated, created_at as CreatedAt
                FROM sensors";

            return await connection.QueryAsync<Device>(sql);
        }
    }
}

