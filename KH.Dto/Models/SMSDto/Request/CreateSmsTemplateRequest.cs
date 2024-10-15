using KH.Domain.Entities;

namespace KH.Dto.Models.SMSDto.Request;

public class CreateSmsTemplateRequest
{
  public long? Id { get; set; }
  public string SmsType { get; set; }
  public string TextEn { get; set; }
  public string TextAr { get; set; }

  public CreateSmsTemplateRequest()
  {

  }
  // Constructor for mapping from SmsTemplate
  public CreateSmsTemplateRequest(SmsTemplate smsTemplate)
  {
    Id = smsTemplate.Id;
    SmsType = smsTemplate.SmsType;
    TextEn = smsTemplate.TextEn;
    TextAr = smsTemplate.TextAr;
  }

  // ToEntity method to convert back to SmsTemplate
  public SmsTemplate ToEntity()
  {
    return new SmsTemplate
    {
      Id = Id.HasValue ? Id.Value : 0,
      SmsType = SmsType,
      TextEn = TextEn,
      TextAr = TextAr
    };
  }
}
