using KH.Domain.Enums;

namespace KH.Helper.Settings
{
  public class MailTemplatesSettings
  {
    public List<MailTemplateType> Types { get; set; }
  }

  public class MailTemplateType
  {
    public MailTypeEnum MailType { get; set; }
    public string? TemplatePath { get; set; }
  }
}
