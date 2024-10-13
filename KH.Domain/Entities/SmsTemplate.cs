using KH.BuildingBlocks.Apis.Entities;

namespace KH.Domain.Entities;

public class SmsTemplate : TrackerEntity
{
  /// <summary>
  /// Refernce Enum [SmsTypeEnum]
  /// </summary>
  public string SmsType { get; set; }

  public string TextEn { get; set; }

  public string TextAr { get; set; }
}
