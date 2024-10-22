
namespace KH.Services.BackgroundJobs.HangfireJobs.Contracts;
public interface IJobTestService
{
  void FireAndForgetJob();

  void ReccuringJob();

  void DelayedJob();

  void ContinuationJob();

}
