using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace OnDemandTutor.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BackgroundJobController : ControllerBase
{
    [HttpPost]
    [Route("create-background-job")]
    public ActionResult CreateBackgroundJob()
    {
        BackgroundJob.Enqueue(() => Console.WriteLine("Hangfire is running"));
        return Ok(); ;
    }

    [HttpPost]
    [Route("create-scheduled-job")]
    public ActionResult CreateScheduledJob()
    {
        var shedulerDateTime = DateTime.UtcNow.AddSeconds(5);
        var dataTimeOffset = new DateTimeOffset(shedulerDateTime);
        var jobId = BackgroundJob.Schedule(() => Console.WriteLine("Scheduled job created"), dataTimeOffset);
        return Ok(); ;
    }

    [HttpPost]
    [Route("create-continuation-job")]
    public ActionResult CreateContinuetionJob()
    {
        var shedulerDateTime = DateTime.UtcNow.AddSeconds(5);
        var dataTimeOffset = new DateTimeOffset(shedulerDateTime);
        var jobId = BackgroundJob.Schedule(() => Console.WriteLine("Scheduled job created"), dataTimeOffset);

        var jobId2 = BackgroundJob.ContinueJobWith(jobId, () => Console.WriteLine($"Continue Job {jobId} 2 times"));
        var jobId3 = BackgroundJob.ContinueJobWith(jobId2, () => Console.WriteLine($"Continue Job {jobId} 3 times"));
        return Ok(); ;
    }

    [HttpPost]
    [Route("create-recurring-job")]
    public ActionResult CreateReCurringJob()
    {
        RecurringJob.AddOrUpdate("RecurringJob", () => Console.WriteLine("RecurringJob is running"), "*/5 * * * *"); // cron for 5 minutes
        return Ok(); ;
    }
}