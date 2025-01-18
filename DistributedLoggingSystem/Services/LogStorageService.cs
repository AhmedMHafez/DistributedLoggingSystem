using DistributedLoggingSystem.DTOs;
using DistributedLoggingSystem.Repositories;

namespace DistributedLoggingSystem.Services
{
    public class LogStorageService : ILogStorageService
    {
        private readonly ILogStorageRepository _logRepository;
        public LogStorageService(ILogStorageRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task StoreLogEntryAsync(LogEntryDto logEntryDto)
        {
            LogEntry logEntry = new LogEntry
            {
                Service = logEntryDto.Service,
                Level = logEntryDto.Level,
                Message = logEntryDto.Message,
                Timestamp = logEntryDto.Timestamp,
            };
            await _logRepository.AddAsync(logEntry);
        }

        public async Task<List<LogEntry>> RetrieveLogEntriesAsync(LogQuery query)
        {
            return await _logRepository.GetLogsAsync(query);
        }
    }

}
