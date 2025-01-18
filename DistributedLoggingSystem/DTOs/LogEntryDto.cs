using System.Reflection.Metadata.Ecma335;

namespace DistributedLoggingSystem.DTOs
{
    public class LogEntryDto
    {
        public string Service { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
