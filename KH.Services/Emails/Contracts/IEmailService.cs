using KH.Dto.Models.EmailDto.Request;

namespace KH.Services.Emails.Contracts;
public interface IEmailService
{
  Task<ApiResponse<string>> SendEmailAsync(MailRequest mailRequest, CancellationToken cancellationToken, bool isResend = false);
  Task<ApiResponse<string>> SendMultipleEmailsAsync(List<MailRequest> mailRequests, CancellationToken cancellationToken);
  Task<ApiResponse<string>> ScheduleEmailAsync(MailRequest mailRequest, DateTime? scheduledTime, CancellationToken cancellationToken);
  Task<ApiResponse<string>> ResendEmailAsync(long emailTrackerId, CancellationToken cancellationToken);
  Task<ApiResponse<string>> ResendRangeOfScheduledEmailsAsync(int batchSize, CancellationToken cancellationToken);

  
}
