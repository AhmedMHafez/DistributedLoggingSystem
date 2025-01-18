using DistributedLoggingSystem.DTOs;
using DistributedLoggingSystem.Services;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace DistributedLoggingSystem.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly ILogStorageService _logStorageService;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public LogsController(ILogStorageService logStorageService, IBackgroundJobClient backgroundJobClient)
        {
            _logStorageService = logStorageService;
            _backgroundJobClient = backgroundJobClient;
        }

        // POST /v1/logs
        [HttpPost]
        public async Task<IActionResult> StoreLog([FromBody] LogEntryDto logEntryDto)
        {

            if (logEntryDto == null)
                return BadRequest("Log entry is null.");

            _backgroundJobClient.Enqueue(() => _logStorageService.StoreLogEntryAsync(logEntryDto));

            return Ok();
        }

        // GET /v1/logs
        [HttpGet]
        public async Task<IActionResult> GetLogs([FromQuery] string? service, [FromQuery] string? level, [FromQuery] DateTime? startTime, [FromQuery] DateTime? endTime)
        {
            var logQuery = new LogQuery
            {
                Service = service,
                Level = level,
                StartTime = startTime,
                EndTime = endTime
            };

            var logs = await _logStorageService.RetrieveLogEntriesAsync(logQuery);
            return Ok(logs);
        }
    }
}
