using KH.Dto.Models.EmailDto.Request;
using KH.Dto.Models.EmailDto.Response;

namespace KH.Services.Emails.Contracts;

public interface IEmailTrackerQueryService
{
  Task<ApiResponse<EmailTrackerResponse>> GetAsync(long id, CancellationToken cancellationToken);
  Task<ApiResponse<PagedList<EmailTrackerResponse>>> GetListAsync(MailRequest request, CancellationToken cancellationToken);

}
