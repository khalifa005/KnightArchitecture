using KH.Dto.Models.EmailDto.Request;
using KH.Dto.Models.EmailDto.Response;

namespace KH.Services.Emails.Contracts;
public interface IEmailService
{
  //Task<ApiResponse<string>> SendEmailAsync(MailRequest mailRequest, CancellationToken cancellationToken);
  Task<ApiResponse<string>> SendEmailAsync(MailRequest mailRequest, CancellationToken cancellationToken, bool isResend = false);
  Task<ApiResponse<EmailTrackerResponse>> GetAsync(long id, CancellationToken cancellationToken);
  Task<ApiResponse<PagedResponse<EmailTrackerResponse>>> GetListAsync(MailRequest request, CancellationToken cancellationToken);
}


public interface IEmailSenderService
{
  Task<ApiResponse<string>> SendEmailAsync(MailRequest mailRequest, CancellationToken cancellationToken, bool isResend = false);
  Task<ApiResponse<string>> SendMultipleEmailsAsync(List<MailRequest> mailRequests, CancellationToken cancellationToken);
}

public interface IEmailTrackingService
{
  Task<ApiResponse<EmailTrackerResponse>> GetAsync(long id, CancellationToken cancellationToken);
  Task<ApiResponse<string>> TrackEmailAsync(MailRequest mailRequest, bool isSent, string failReasons, CancellationToken cancellationToken);
  Task<ApiResponse<string>> ResendEmailAsync(long emailTrackerId, CancellationToken cancellationToken);
  Task<ApiResponse<string>> ResendMissedEmailsAsync(int batchSize, CancellationToken cancellationToken);
  Task<ApiResponse<PagedResponse<EmailTrackerResponse>>> GetPagedEmailTrackersAsync(MailRequest request, CancellationToken cancellationToken);
}
