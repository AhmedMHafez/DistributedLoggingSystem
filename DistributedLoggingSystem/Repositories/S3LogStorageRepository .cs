using DistributedLoggingSystem.DTOs;
using Newtonsoft.Json;
using System.Text;

namespace DistributedLoggingSystem.Repositories
{
    public class S3LogRepository : ILogStorageRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _s3Url;
        private readonly string _bucketName;

        public S3LogRepository(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _s3Url = configuration["LoggingBackend:S3Url"]; ; ;
            _bucketName = configuration["LoggingBackend:BucketName"]; ; ;
        }

        public async Task AddAsync(LogEntry logEntry)
        {
            var content = new StringContent(JsonConvert.SerializeObject(logEntry), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_s3Url}/{_bucketName}/logs/{Guid.NewGuid()}.json", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to log to S3.");
            }
        }

        public async Task<List<LogEntry>> GetLogsAsync(LogQuery logQuery)
        {
            // Assuming the logs are stored in S3 as JSON files.
            // A more realistic scenario would involve listing objects and querying metadata (if supported).

            var logs = new List<LogEntry>();

            // Mock behavior: Here you would perform HTTP GET requests to retrieve logs
            // and filter by service, level, time range in a similar manner.

            return logs;
        }
    }

}
