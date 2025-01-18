using Microsoft.EntityFrameworkCore;

namespace DistributedLoggingSystem.Models
{
    public class LogContext : DbContext
    {
        public DbSet<LogEntry> LogEntries { get; set; }

        public LogContext(DbContextOptions<LogContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<LogEntry>().ToTable("Logs");
        }
    }

}
