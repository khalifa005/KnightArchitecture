using KH.BuildingBlocks.Common.Enums;
using KH.Dto.Models.SMSDto.Request;
using KH.Dto.Models.SMSDto.Response;

namespace KH.Services.Sms.Contracts;

public interface ISmsTemplateService
{
  Task<ApiResponse<SmsTemplateResponse>> GetSmsTemplateAsync(string smsType, CancellationToken cancellationToken);
  Task<ApiResponse<PagedList<SmsTemplateResponse>>> GetSmsTemplateListAsync(SmsTrackerFilterRequest request, CancellationToken cancellationToken);
  Task<ApiResponse<string>> AddSmsTemplateAsync(CreateSmsTemplateRequest request, CancellationToken cancellationToken);
  Task<ApiResponse<string>> DeleteAsync(long id, CancellationToken cancellationToken);
  Task<ApiResponse<string>> UpdateAsync(CreateSmsTemplateRequest request, CancellationToken cancellationToken);

  string ReplaceWelcomeSmsPlaceholders(string template, User user);
  string GetTemplateForLanguage(SmsTemplateResponse smsTemplate, LanguageEnum language);
}
