using KH.Dto.Models.SMSDto.Request;
using KH.Dto.Models.SMSDto.Response;

namespace KH.Services.Contracts;

public interface ISmsTemplateService
{
  Task<ApiResponse<SmsTemplateResponse>> GetSmsTemplateAsync(string smsType);
  Task<ApiResponse<PagedResponse<SmsTemplateResponse>>> GetSmsTemplateListAsync(SmsTrackerFilterRequest request);
  Task<ApiResponse<string>> AddSmsTemplateAsync(SmsTemplateForm request);
  Task<ApiResponse<string>> DeleteAsync(long id);
  Task<ApiResponse<string>> UpdateAsync(SmsTemplateForm request);

  string ReplaceWelcomeSmsPlaceholders(string template, User user);
  string GetTemplateForLanguage(SmsTemplateResponse smsTemplate, LanguageEnum language);
}
