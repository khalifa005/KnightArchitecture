using System.Diagnostics.CodeAnalysis;

namespace KH.BuildingBlocks.Services;

[ExcludeFromCodeCoverage]
public class ChangedEventArgs
{
  public string Key { get; set; }
  public object OldValue { get; set; }
  public object NewValue { get; set; }
}
