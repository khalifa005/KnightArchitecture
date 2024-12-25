namespace KH.BuildingBlocks.Apis.Extentions;

public static class NumberExtensions
{
  public static bool IsNotEmptyNumber(this object? val)
  {
    try
    {
      if (val is null)
        return false;

      var valStr = val.ToString();

      if (!valStr.IsNotEmpty())
        return false;

      if (int.TryParse(valStr, out int valInt))
        return valInt != 0;

      return false;
    }
    catch
    {
      return false;
    }
  }
}
