using Hangfire;
using KH.BuildingBlocks.Auth.Constant;
using KH.BuildingBlocks.Auth.Enum;
using KH.Services.Audits.Contracts;
using KH.Services.Audits.Implementation;
using KH.Services.BackgroundJobs.HangfireJobs.Contracts;
using System.Net;

namespace KH.WebApi.Controllers;

public class JobTestController : BaseApiController
{
  private readonly IJobTestService _jobTestService;
  private readonly IBackgroundJobClient _backgroundJobClient;
  private readonly IRecurringJobManager _recurringJobManager;
  public JobTestController(IJobTestService jobTestService, IBackgroundJobClient backgroundJobClient, IRecurringJobManager recurringJobManager)
  {
    _jobTestService = jobTestService;
    _backgroundJobClient = backgroundJobClient;
    _recurringJobManager = recurringJobManager;
  }

  [HttpGet("FireAndForgetJob")]
  public async Task<ActionResult> FireAndForgetJob()
  {
    _backgroundJobClient.Enqueue(() => _jobTestService.FireAndForgetJob());
    return Ok();
  }

  [HttpGet("DelayedJob")]
  public async Task<ActionResult> CreateDelayedJob()
  {
    _backgroundJobClient.Schedule(() => _jobTestService.DelayedJob(), TimeSpan.FromSeconds(60));
    return Ok();
  }

  [HttpGet("ReccuringJob")]
  public async Task<ActionResult> CreateReccuringJob()
  {
    _recurringJobManager.AddOrUpdate("jobId", () => _jobTestService.ReccuringJob(), Cron.Minutely);
    return Ok();
  }

  [HttpGet("ContinuationJob")]
  public async Task<ActionResult> CreateContinuationJob()
  {
    var parentJobId = _backgroundJobClient.Enqueue(() => _jobTestService.FireAndForgetJob());
    _backgroundJobClient.ContinueJobWith(parentJobId, () => _jobTestService.ContinuationJob());

    return Ok();
  }
}

