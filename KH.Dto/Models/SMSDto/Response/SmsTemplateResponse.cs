using KH.Domain.Entities;

namespace KH.Dto.Models.SMSDto.Response;

public class SmsTemplateResponse : BasicTrackerEntityDto
{
  public string SmsType { get; set; }
  public string TextEn { get; set; }
  public string TextAr { get; set; }

  public SmsTemplateResponse()
  {

  }

  public SmsTemplateResponse(SmsTemplate smsTemplate)
  {
    SmsType = smsTemplate.SmsType;
    TextEn = smsTemplate.TextEn;
    TextAr = smsTemplate.TextAr;
    CreatedDate = smsTemplate.CreatedDate;
    CreatedById = smsTemplate.CreatedById;
    UpdatedDate = smsTemplate.UpdatedDate;
    UpdatedById = smsTemplate.UpdatedById;
    IsDeleted = smsTemplate.IsDeleted;
  }
}
