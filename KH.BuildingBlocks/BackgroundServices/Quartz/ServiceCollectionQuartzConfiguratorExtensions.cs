using Quartz;

namespace KH.Services;

using Quartz;

  public static class ServiceCollectionQuartzConfiguratorExtensions
  {
    public static void AddJobAndTrigger<T>(
        this IServiceCollectionQuartzConfigurator quartz,
        IConfiguration config,
        bool immediateExecution = false)
        where T : IJob
    {
      // Use the name of the IJob as the appsettings.json key
      string jobName = typeof(T).Name;
      var configKey = $"Quartz:Jobs:{jobName}";

      // Register the job as before
      var jobKey = new JobKey(jobName);
      quartz.AddJob<T>(opts => opts.WithIdentity(jobKey));

      // Check if StartNow is configured in appsettings
      var startNow = config.GetValue<bool>($"{configKey}:StartNow");

      if (immediateExecution || startNow)
      {
        quartz.AddTrigger(opts => opts
            .ForJob(jobKey)
            .WithIdentity(jobName + "-trigger")
            .StartNow());
      }
      else
      {
        var cronSchedule = config[$"{configKey}:CronSchedule"];
        var intervalInMinutes = config.GetValue<int?>($"{configKey}:IntervalInMinutes");

        // Validation for cron or interval schedule
        if (!string.IsNullOrEmpty(cronSchedule))
        {
          quartz.AddTrigger(opts => opts
              .ForJob(jobKey)
              .WithIdentity(jobName + "-trigger")
              .WithCronSchedule(cronSchedule)); // Use cron-based schedule
        }
        else if (intervalInMinutes.HasValue)
        {
          quartz.AddTrigger(opts => opts
              .ForJob(jobKey)
              .WithIdentity(jobName + "-trigger")
              .WithSimpleSchedule(x => x
                  .WithIntervalInMinutes(intervalInMinutes.Value)
                  .RepeatForever())); // Use simple schedule
        }
        else
        {
          throw new Exception($"No valid schedule found for job in configuration at {configKey}");
        }
      }
    }
  }
