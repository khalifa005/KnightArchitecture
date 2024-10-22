using KH.Services.BackgroundJobs.HangfireJobs.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KH.Services.BackgroundJobs.HangfireJobs.Implementation;
public class JobTestService : IJobTestService
{
  private ILogger _logger;

  public JobTestService(ILogger<JobTestService> logger)
  {
    _logger = logger;
  }

  public void ContinuationJob()
  {
    _logger.LogInformation("Hello from a Continuation job!");
  }

  public void DelayedJob()
  {
    _logger.LogInformation("Hello from a Delayed job!");
  }

  public void FireAndForgetJob()
  {
    _logger.LogInformation("Hello from a Fire and Forget job!");
  }

  public void ReccuringJob()
  {
    _logger.LogInformation("Hello from a Scheduled job!");
  }
}
