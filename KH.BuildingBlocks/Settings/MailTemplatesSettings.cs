using KH.BuildingBlocks.Apis.Enums;

namespace KH.BuildingBlocks.Settings;

public class MailTemplatesSettings
{
  public List<MailTemplateType> Types { get; set; }
}

public class MailTemplateType
{
  public MailTypeEnum MailType { get; set; }
  public string? TemplatePath { get; set; }
}
