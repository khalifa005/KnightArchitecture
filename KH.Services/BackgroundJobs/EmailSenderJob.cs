using KH.Services.Emails.Contracts;

namespace KH.Services.BackgroundServices;
[DisallowConcurrentExecution]
public class EmailSenderJob : IJob
{
  private readonly ILogger<EmailSenderJob> _logger;
  private readonly IEmailService _mailService;

  private readonly IUnitOfWork _unitOfWork;
  const string TaskName = nameof(EmailSenderJob);
  private long _iteration;
  public EmailSenderJob(
      IUnitOfWork unitOfWork,
      IEmailService mailService,
      ILogger<EmailSenderJob> logger)
  {
    _unitOfWork = unitOfWork;
    _logger = logger;
    _mailService = mailService;
  }

  public async Task Execute(IJobExecutionContext context)
  {
    _logger.LogInformation("{Task} Service is starting.", TaskName);

    try
    {
      var startTime = DateTime.Now;
      _logger.LogInformation("{Task} Execution started at: {StartTime}", TaskName, startTime);

      // Delay the execution for 5 minutes (300,000 milliseconds)
      await Task.Delay(TimeSpan.FromMinutes(5));

      var endTime = DateTime.Now;
      _logger.LogInformation("{Task} Execution resumed after delay at: {EndTime}", TaskName, endTime);

      var currentDate = DateTime.Now;

      // Your logic here after the delay

    }
    catch (Exception ex)
    {
      _logger.LogError("{Task} has exception {Exception}", TaskName, ex.Message);
    }
  }

}