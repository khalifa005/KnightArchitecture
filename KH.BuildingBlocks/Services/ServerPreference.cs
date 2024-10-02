namespace KH.BuildingBlocks.Services;

public record ServerPreference : IPreference
{
  public string LanguageCode { get; set; } = LocalizationConstants.SupportedLanguages.FirstOrDefault()?.Code ?? "en-US";

  //TODO - add server preferences
}
