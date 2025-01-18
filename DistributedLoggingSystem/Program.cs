using DistributedLoggingSystem.Models;
using DistributedLoggingSystem.Repositories;
using DistributedLoggingSystem.Services;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add builder.Services to the container.
builder.Services.AddControllers();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170).UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings().UseSqlServerStorage(connectionString, new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));

builder.Services.AddHangfireServer();
// Configure DbContext for SQL Server
builder.Services.AddDbContext<LogContext>(options =>
    options.UseSqlServer(connectionString));

// Add Authorization Policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("User", policy => policy.RequireRole("User"));
});

// Register custom builder.Services
builder.Services.AddScoped<ILogStorageService, LogStorageService>();
var backend = builder.Configuration["LoggingBackend:Backend"];

switch (backend.ToLower())
{
    case "database":
        builder.Services.AddScoped<ILogStorageRepository, DatabaseLogStorageRepository>();
        break;
    case "s3":
        builder.Services.AddScoped<ILogStorageRepository, S3LogRepository>();
        break;
    case "file":
        builder.Services.AddScoped<ILogStorageRepository, LocalFileLogRepository>();
        break;
    case "messagequeue":
        builder.Services.AddScoped<ILogStorageRepository, MQLogRepository>();
        break;
    default:
        throw new InvalidOperationException("unsupported storage backend.");
        break;
}



// Build the app
var app = builder.Build();
app.UseHangfireDashboard("/hangfire");

app.UseHangfireServer();


// Configure the HTTP request pipeline.
app.UseAuthentication();
app.UseAuthorization();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();

app.Run();
