using KH.Domain.Entities;

namespace KH.Dto.Models.SMSDto.Form;

public class SmsTemplateForm
{
  public long? Id { get; set; }
  public string SmsType { get; set; }
  public string TextEn { get; set; }
  public string TextAr { get; set; }

  public SmsTemplateForm()
  {

  }
  // Constructor for mapping from SmsTemplate
  public SmsTemplateForm(SmsTemplate smsTemplate)
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
      Id = this.Id.HasValue ? this.Id.Value : 0,
      SmsType = this.SmsType,
      TextEn = this.TextEn,
      TextAr = this.TextAr
    };
  }
}
