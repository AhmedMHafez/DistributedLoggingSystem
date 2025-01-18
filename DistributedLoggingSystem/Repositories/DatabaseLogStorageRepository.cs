using DistributedLoggingSystem.DTOs;
using DistributedLoggingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace DistributedLoggingSystem.Repositories
{
    public class DatabaseLogStorageRepository : ILogStorageRepository
    {
        private readonly LogContext _context;

        public DatabaseLogStorageRepository(LogContext context)
        {
            _context = context;
        }

        public async Task<List<LogEntry>> GetLogsAsync(LogQuery logQuery)
        {
            var queryable = _context.LogEntries.AsQueryable();

            if (!string.IsNullOrEmpty(logQuery.Service))
            {
                queryable = queryable.Where(log => log.Service == logQuery.Service);
            }

            if (!string.IsNullOrEmpty(logQuery.Level))
            {
                queryable = queryable.Where(log => log.Level == logQuery.Level);
            }

            if (logQuery.StartTime.HasValue)
            {
                queryable = queryable.Where(log => log.Timestamp >= logQuery.StartTime.Value);
            }

            if (logQuery.EndTime.HasValue)
            {
                queryable = queryable.Where(log => log.Timestamp <= logQuery.EndTime.Value);
            }

            return await queryable.ToListAsync();
        }

        public async Task AddAsync(LogEntry log)
        {
            _context.LogEntries.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}
