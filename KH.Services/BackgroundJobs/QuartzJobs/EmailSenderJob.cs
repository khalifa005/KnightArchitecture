using KH.Dto.Models.EmailDto.Request;
using KH.Services.Emails.Contracts;

namespace KH.Services.BackgroundJobs.QuartzJobs;
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
      //MailRequest.
      //var emailListResponse = await _mailService.GetListAsync();

    }
    catch (Exception ex)
    {
      _logger.LogError("{Task} has exception {Exception}", TaskName, ex.Message);
    }
  }

}
