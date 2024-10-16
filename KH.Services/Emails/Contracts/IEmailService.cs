using KH.Dto.Models.EmailDto.Request;
using KH.Dto.Models.EmailDto.Response;

namespace KH.Services.Emails.Contracts;
public interface IEmailService
{
  Task<ApiResponse<string>> SendEmailAsync(MailRequest mailRequest, CancellationToken cancellationToken);
  Task<ApiResponse<EmailTrackerResponse>> GetAsync(long id, CancellationToken cancellationToken);
  Task<ApiResponse<PagedResponse<EmailTrackerResponse>>> GetListAsync(MailRequest request, CancellationToken cancellationToken);
}


