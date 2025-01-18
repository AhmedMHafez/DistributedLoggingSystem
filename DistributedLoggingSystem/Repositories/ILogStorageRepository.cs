using DistributedLoggingSystem.DTOs;

namespace DistributedLoggingSystem.Repositories
{
    public interface ILogStorageRepository
    {
        Task<List<LogEntry>> GetLogsAsync(LogQuery logQuery);
        Task AddAsync(LogEntry log);
    }
}
