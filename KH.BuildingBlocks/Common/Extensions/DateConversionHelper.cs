namespace KH.BuildingBlocks.Apis.Extentions;

public static class DateConversionHelper
{
  public static DateTime? ConvertHijriToGregorian(string hijriDate)
  {
    DateTime time;
    string[] formats = { "dd/MM/yyyy", "d/MM/yyyy", "dd/M/yyyy", "d/M/yyyy" };

    if (DateTime.TryParseExact(
        hijriDate,
        formats,
        new CultureInfo("ar-SA").DateTimeFormat,
        DateTimeStyles.AllowInnerWhite,
        out time))
    {
      return time;
    }
    return null;
  }
}

