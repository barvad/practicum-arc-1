using Dapper;
using SmartHome.DeviceRegistryService.Domain.Entities;
using SmartHome.DeviceRegistryService.Domain.Interfaces;
using SmartHome.DeviceRegistryService.Infrastructure.Data;

namespace SmartHome.DeviceRegistryService.Infrastructure.Data
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly IDatabaseConnectionFactory _connectionFactory;

        public DeviceRepository(IDatabaseConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Device>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            
            const string sql = @"
                SELECT id, name, type, location, value, unit, status, last_updated as LastUpdated, created_at as CreatedAt
                FROM sensors
                ORDER BY id";

            return await connection.QueryAsync<Device>(sql);
        }

        public async Task<Device?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            
            const string sql = @"
                SELECT id, name, type, location, value, unit, status, last_updated as LastUpdated, created_at as CreatedAt
                FROM sensors 
                WHERE id = @Id";

            return await connection.QuerySingleOrDefaultAsync<Device>(sql, new { Id = id });
        }

        public async Task<Device> CreateAsync(Device device)
        {
            using var connection = _connectionFactory.CreateConnection();
            
            const string sql = @"
                INSERT INTO sensors (name, type, location, value, unit, status, last_updated, created_at)
                VALUES (@Name, @Type, @Location, @Value, @Unit, @Status, @LastUpdated, @CreatedAt)
                RETURNING id, name, type, location, value, unit, status, last_updated as LastUpdated, created_at as CreatedAt";

            return await connection.QuerySingleAsync<Device>(sql, device);
        }

        public async Task UpdateAsync(Device device)
        {
            using var connection = _connectionFactory.CreateConnection();
            
            const string sql = @"
                UPDATE sensors 
                SET name = @Name, type = @Type, location = @Location, value = @Value, 
                    unit = @Unit, status = @Status, last_updated = @LastUpdated
                WHERE id = @Id";

            await connection.ExecuteAsync(sql, device);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            
            const string sql = "DELETE FROM sensors WHERE id = @Id";

            await connection.ExecuteAsync(sql, new { Id = id });
        }

        public async Task<bool> ExistsAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            
            const string sql = "SELECT COUNT(1) FROM sensors WHERE id = @Id";

            var count = await connection.ExecuteScalarAsync<int>(sql, new { Id = id });
            return count > 0;
        }
    }
}