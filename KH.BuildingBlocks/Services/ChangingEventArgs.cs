using System.Diagnostics.CodeAnalysis;

namespace KH.BuildingBlocks.Services;

[ExcludeFromCodeCoverage]
public class ChangingEventArgs : ChangedEventArgs
{
  public bool Cancel { get; set; }
}
