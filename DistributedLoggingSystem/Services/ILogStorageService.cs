using DistributedLoggingSystem.DTOs;

namespace DistributedLoggingSystem.Services
{
    public interface ILogStorageService
    {
        Task StoreLogEntryAsync(LogEntryDto logEntry);
        Task<List<LogEntry>> RetrieveLogEntriesAsync(LogQuery logQuery);

    }
}
