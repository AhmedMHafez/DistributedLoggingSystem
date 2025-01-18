using DistributedLoggingSystem.DTOs;
using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json;
using System.Text;

namespace DistributedLoggingSystem.Repositories
{
    public class MQLogRepository : ILogStorageRepository
    {
        private readonly IModel _channel;

        public MQLogRepository(IModel channel)
        {
            _channel = channel;
        }

        public async Task AddAsync(LogEntry logEntry)
        {
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(logEntry));
            //_channel.BasicPublish(exchange: "", routingKey: "logs", basicProperties: null, body: body);
            await Task.CompletedTask;
        }

        public async Task<List<LogEntry>> GetLogsAsync(LogQuery logQuery)
        {
            // Message queues typically don't store logs, but we could simulate consuming logs based on time.
            return await Task.FromResult(new List<LogEntry>());
        }
    }

}
