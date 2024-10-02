namespace KH.BuildingBlocks.Services;

public interface IPreferenceManager
{
  Task SetPreference(IPreference preference);

  Task<IPreference> GetPreference();

  Task<IResult> ChangeLanguageAsync(string languageCode);
}
