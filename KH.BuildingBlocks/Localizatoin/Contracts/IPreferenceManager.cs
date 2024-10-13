using KH.BuildingBlocks.Apis.Contracts;
using IResult = KH.BuildingBlocks.Apis.Contracts.IResult;

namespace KH.BuildingBlocks.Localizatoin.Contracts;

public interface IPreferenceManager
{
  Task SetPreference(IPreference preference);

  Task<IPreference> GetPreference();

  Task<IResult> ChangeLanguageAsync(string languageCode);
}
