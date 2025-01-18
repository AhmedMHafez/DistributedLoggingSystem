namespace DistributedLoggingSystem.DTOs
{
    public class LogQuery
    {
        public string Service { get; set; }
        public string Level { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }

}
