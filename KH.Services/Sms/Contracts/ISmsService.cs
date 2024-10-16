using KH.Dto.Models.SMSDto.Request;
using KH.Dto.Models.SMSDto.Response;

namespace KH.Services.Sms.Contracts;

public interface ISmsService
{
  string BuildFormattedSmsApiUrl(string smsApiUrl, string userName, string password, string phoneNumber, string message);
  Task<ApiResponse<SmsTrackerResponse>> GetSmsTrackerAsync(long id, CancellationToken cancellationToken);
  Task<ApiResponse<PagedResponse<SmsTrackerResponse>>> GetSmsTrackerListAsync(SmsTrackerFilterRequest request, CancellationToken cancellationToken);
  Task<ApiResponse<string>> SendSmsAsync(CreateSmsTrackerRequest request, CancellationToken cancellationToken);
  Task<ApiResponse<string>> ResendAsync(SmsTracker request, CancellationToken cancellationToken);
}
