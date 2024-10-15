using KH.Dto.Models.SMSDto.Request;
using KH.Dto.Models.SMSDto.Response;

namespace KH.Services.Sms.Contracts;

public interface ISmsService
{
  string BuildFormattedSmsApiUrl(string smsApiUrl, string userName, string password, string phoneNumber, string message);
  Task<ApiResponse<SmsTrackerResponse>> GetSmsTrackerAsync(long id);
  Task<ApiResponse<PagedResponse<SmsTrackerResponse>>> GetSmsTrackerListAsync(SmsTrackerFilterRequest request);
  Task<ApiResponse<string>> SendSmsAsync(CreateSmsTrackerRequest request);
  Task<ApiResponse<string>> ResendAsync(SmsTracker request);
}
