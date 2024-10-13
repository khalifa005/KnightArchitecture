using KH.BuildingBlocks.Localizatoin.Constants;
using KH.BuildingBlocks.Localizatoin.Contracts;

namespace KH.BuildingBlocks.Localizatoin.Services;

public record ServerPreference : IPreference
{
  public string LanguageCode { get; set; } = LocalizationConstants.SupportedLanguages.FirstOrDefault()?.Code ?? "en-US";

  //TODO - add server preferences
}
