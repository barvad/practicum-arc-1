using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using SmartHome.DeviceStatusService.Domain.Entities;
using SmartHome.DeviceStatusService.Domain.Interfaces;
using SmartHome.DeviceStatusService.Domain.Models;

namespace SmartHome.DeviceStatusService.API.Kafka
{
    public class DeviceCommandConsumerService : BackgroundService
    {
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<DeviceCommandConsumerService> _logger;
        private readonly string _topic;

        public DeviceCommandConsumerService(
            IOptions<KafkaSettings> kafkaSettings,
            IServiceScopeFactory serviceScopeFactory,
            ILogger<DeviceCommandConsumerService> logger)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = kafkaSettings.Value.BootstrapServers,
                GroupId = kafkaSettings.Value.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
            _topic = kafkaSettings.Value.Topic;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.Subscribe(_topic);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = _consumer.Consume(stoppingToken);
                    
                    if (consumeResult?.Message?.Value != null)
                    {
                        await ProcessMessage(consumeResult.Message.Value);
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing Kafka message");
                }
            }

            _consumer.Close();
        }

        private async Task ProcessMessage(string message)
        {
            try
            {
                var deviceCommand = JsonSerializer.Deserialize<DeviceCommand>(message);
                
                if (deviceCommand != null)
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var deviceStatusService = scope.ServiceProvider.GetRequiredService<IDeviceStatusService>();
                    
                    await deviceStatusService.ProcessDeviceCommand(deviceCommand);
                    _logger.LogInformation("Processed device command for device {DeviceId}", deviceCommand.DeviceId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deserializing or processing device command message: {Message}", message);
            }
        }

        public override void Dispose()
        {
            _consumer?.Dispose();
            base.Dispose();
        }
    }

 
}

