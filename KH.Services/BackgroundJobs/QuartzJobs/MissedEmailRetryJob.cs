using KH.Services.Emails.Contracts;

namespace KH.Services.BackgroundJobs.QuartzJobs;
[DisallowConcurrentExecution]
public class MissedEmailRetryJob : IJob
{
  private readonly ILogger<MissedEmailRetryJob> _logger;
  private readonly IEmailService _mailService;

  private readonly IUnitOfWork _unitOfWork;
  const string TaskName = nameof(MissedEmailRetryJob);
  private long _iteration;
  public MissedEmailRetryJob(
      IUnitOfWork unitOfWork,
      IEmailService mailService,
      ILogger<MissedEmailRetryJob> logger)
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
      var emailListResponse = await _mailService.ResendRangeOfMissedEmailsAsync(batchSize:10, cancellationToken: cancellationToken);
    }
    catch (Exception ex)
    {
      _logger.LogError("{Task} has exception {Exception}", TaskName, ex.Message);
    }
  }

}
