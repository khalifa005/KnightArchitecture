using KH.BuildingBlocks.Apis.Responses;
using KH.Dto.Models.EmailDto.Request;
using KH.Dto.Models.EmailDto.Response;

namespace KH.Services.Contracts;
public interface IEmailService
{
  Task<ApiResponse<object>> SendEmailAsync(MailRequest mailRequest);
  Task<ApiResponse<EmailTrackerResponse>> GetAsync(long id);
  Task<ApiResponse<PagedResponse<EmailTrackerResponse>>> GetListAsync(MailRequest request);
  //Task<ApiResponse<string>> AddAsync(EmailTracker request);
}


