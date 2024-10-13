using Microsoft.Extensions.Localization;

namespace KH.BuildingBlocks.Localizatoin.Services;

internal class ServerLocalizer<T> where T : class
{
  public IStringLocalizer<T> Localizer { get; }

  public ServerLocalizer(IStringLocalizer<T> localizer)
  {
    Localizer = localizer;
  }
}
