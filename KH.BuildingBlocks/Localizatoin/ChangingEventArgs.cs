using System.Diagnostics.CodeAnalysis;

namespace KH.BuildingBlocks.Localizatoin;

[ExcludeFromCodeCoverage]
public class ChangingEventArgs : ChangedEventArgs
{
  public bool Cancel { get; set; }
}
