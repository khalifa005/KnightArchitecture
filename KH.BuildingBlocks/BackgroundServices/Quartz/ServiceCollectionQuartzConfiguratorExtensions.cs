using Quartz;

namespace KH.Services;

public static class ServiceCollectionQuartzConfiguratorExtensions
{
  public static void AddJobAndTrigger<T>(
      this IServiceCollectionQuartzConfigurator quartz,
      IConfiguration config,
      ILogger<T> logger,
      bool immidateExecution = false)
      where T : IJob
  {
    // Use the name of the IJob as the appsettings.json key
    string jobName = typeof(T).Name;

    // Try and load the schedule from configuration
    var configKey = $"Quartz:{jobName}";


    // register the job as before
    var jobKey = new JobKey(jobName);
    quartz.AddJob<T>(opts => opts.WithIdentity(jobKey));

    if (immidateExecution)
    {
      quartz.AddTrigger(opts => opts
          .ForJob(jobKey)
          .WithIdentity(jobName + "-trigger")
          .StartNow());
    }
    else
    {
      var cronSchedule = config[configKey];//from appsetting

      // Some minor validation
      if (string.IsNullOrEmpty(cronSchedule))
      {
        logger.LogError($"cronSchedule key in app seeting does not exist {cronSchedule}");
        //LOG AND SEND EMAIL
        //throw new Exception($"No Quartz.NET Cron schedule found for job in configuration at {configKey}");
      }

      quartz.AddTrigger(opts => opts
      .ForJob(jobKey)
      .WithIdentity(jobName + "-trigger")
      .WithCronSchedule(cronSchedule)); // use the schedule from configuration
    }
  }

}
