using DistributedLoggingSystem;
using DistributedLoggingSystem.DTOs;
using DistributedLoggingSystem.Repositories;
using Newtonsoft.Json;

public class LocalFileLogRepository : ILogStorageRepository
{
    private readonly string _logDirectoryPath;
    private readonly IConfiguration configuration;

    public LocalFileLogRepository(IConfiguration configuration)
    {
        this.configuration = configuration;
        _logDirectoryPath = configuration["LoggingBackend:FilePath"]; ;

    }

    // Add a new log entry to the file (log entry is appended to the file with the service name and date)
    public async Task AddAsync(LogEntry logEntry)
    {
        // Ensure the logs directory exists
        if (!Directory.Exists(_logDirectoryPath))
        {
            Directory.CreateDirectory(_logDirectoryPath);
        }

        // Create the log file name based on the timestamp and service (e.g., logs-2025-01-18-MyService.txt)
        string logFileName = $"logs-{logEntry.Timestamp:yyyy-MM-dd}-{logEntry.Service}.txt";
        string logFilePath = Path.Combine(_logDirectoryPath, logFileName);

        // Format the log entry (timestamp|level|service|message)
        string logLine = $"{logEntry.Timestamp:yyyy-MM-dd HH:mm:ss}|{logEntry.Level}|{logEntry.Service}|{logEntry.Message}";

        // Append the log entry to the file
        await File.AppendAllTextAsync(logFilePath, logLine + Environment.NewLine);
    }

    // Retrieve log entries based on the query parameters
    public async Task<List<LogEntry>> GetLogsAsync(LogQuery logQuery)
    {
        // Ensure the logs directory exists
        if (!Directory.Exists(_logDirectoryPath))
        {
            return Enumerable.Empty<LogEntry>().ToList();
        }

        // Find all log files in the directory (e.g., logs-2025-01-18-MyService.txt)
        var logFiles = Directory.GetFiles(_logDirectoryPath, $"logs-{logQuery.StartTime?.ToString("yyyy-MM-dd") ?? "*"}-{logQuery.Service ?? "*"}*.txt");

        var logEntries = new List<LogEntry>();

        foreach (var logFile in logFiles)
        {
            var lines = await File.ReadAllLinesAsync(logFile);

            foreach (var line in lines)
            {
                // Parse the log entry (timestamp|level|service|message)
                var parts = line.Split('|');
                if (parts.Length == 4)
                {
                    var timestamp = DateTime.Parse(parts[0]);
                    var level = parts[1];
                    var service = parts[2];
                    var message = parts[3];

                    var logEntry = new LogEntry
                    {
                        Timestamp = timestamp,
                        Level = level,
                        Service = service,
                        Message = message
                    };

                    // Apply filters from the query
                    if ((logQuery.Service == null || logEntry.Service.Equals(logQuery.Service, StringComparison.OrdinalIgnoreCase)) &&
                        (logQuery.Level == null || logEntry.Level.Equals(logQuery.Level, StringComparison.OrdinalIgnoreCase)) &&
                        (logQuery.StartTime == null || logEntry.Timestamp >= logQuery.StartTime) &&
                        (logQuery.EndTime == null || logEntry.Timestamp <= logQuery.EndTime))
                    {
                        logEntries.Add(logEntry);
                    }
                }
            }
        }

        return logEntries;
    }
}
