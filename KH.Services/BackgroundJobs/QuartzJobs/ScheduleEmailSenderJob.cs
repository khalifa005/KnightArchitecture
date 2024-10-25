using KH.Services.Emails.Contracts;

namespace KH.Services.BackgroundJobs.QuartzJobs;
[DisallowConcurrentExecution]
public class ScheduleEmailSenderJob : IJob
{
  private readonly ILogger<ScheduleEmailSenderJob> _logger;
  private readonly IEmailService _mailService;

  private readonly IUnitOfWork _unitOfWork;
  const string TaskName = nameof(ScheduleEmailSenderJob);
  private long _iteration;
  public ScheduleEmailSenderJob(
      IUnitOfWork unitOfWork,
      IEmailService mailService,
      ILogger<ScheduleEmailSenderJob> logger)
  {
    _unitOfWork = unitOfWork;
    _logger = logger;
    _mailService = mailService;
  }

  public async Task Execute(IJobExecutionContext context)
  {
    _logger.LogInformation("{Task} Service is starting.", TaskName);
    var cancellationToken = context.CancellationToken;

    try
    {
      var emailListResponse = await _mailService.ResendRangeOfScheduledEmailsAsync(batchSize:10, cancellationToken: cancellationToken);
    }
    catch (Exception ex)
    {
      _logger.LogError("{Task} has exception {Exception}", TaskName, ex.Message);
    }
  }

}
