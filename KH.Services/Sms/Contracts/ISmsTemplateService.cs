using KH.Dto.Models.SMSDto.Request;
using KH.Dto.Models.SMSDto.Response;

namespace KH.Services.Sms.Contracts;

public interface ISmsTemplateService
{
  Task<ApiResponse<SmsTemplateResponse>> GetSmsTemplateAsync(string smsType);
  Task<ApiResponse<PagedResponse<SmsTemplateResponse>>> GetSmsTemplateListAsync(SmsTrackerFilterRequest request);
  Task<ApiResponse<string>> AddSmsTemplateAsync(CreateSmsTemplateRequest request);
  Task<ApiResponse<string>> DeleteAsync(long id);
  Task<ApiResponse<string>> UpdateAsync(CreateSmsTemplateRequest request);

  string ReplaceWelcomeSmsPlaceholders(string template, User user);
  string GetTemplateForLanguage(SmsTemplateResponse smsTemplate, LanguageEnum language);
}
