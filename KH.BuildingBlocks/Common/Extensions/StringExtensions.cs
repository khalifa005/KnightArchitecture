namespace KH.BuildingBlocks.Apis.Extentions;

public static class StringExtensions
{
  public static string? TrimValue(this string? val)
  {
    if (string.IsNullOrEmpty(val) || string.IsNullOrWhiteSpace(val))
      return null;

    val = val.Trim();

    if (val == "null" || val == "undefined")
      return null;

    return val;
  }

  public static bool IsNotEmpty(this string? val)
  {
    try
    {
      return !string.IsNullOrEmpty(val?.TrimValue());
    }
    catch
    {
      return false;
    }
  }
}
