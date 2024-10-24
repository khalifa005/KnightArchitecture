using KH.BuildingBlocks.Apis.Constant;
using Microsoft.AspNetCore.Components;
namespace KH.BuildingBlocks.Apis.Extentions;

public static class CommonHelperMethods
{
  public static string? TrimValue(this string? val)
  {
    // Check for null or empty values and return null if found, otherwise trim the value
    if (string.IsNullOrEmpty(val) || string.IsNullOrWhiteSpace(val))
      return null;

    val = val.Trim();

    // Check for special cases ("null", "undefined")
    if (val == "null" || val == "undefined")
      return null;

    return val;
  }

  public static bool IsNotEmpty(this string? val)
  {
    try
    {
      // If the trimmed value is null or empty, return false
      if (string.IsNullOrEmpty(val?.TrimValue()))
        return false;

      return true;
    }
    catch
    {
      return false;
    }
  }

  public static bool IsNotEmptyNumber(this object? val)
  {
    try
    {
      // Return false if val is null
      if (val is null)
        return false;

      var valStr = val.ToString();

      // Return false if the string value is null or empty
      if (!valStr.IsNotEmpty())
        return false;

      // Attempt to parse the value as an integer
      if (int.TryParse(valStr, out int valInt))
      {
        // Return false if the integer is 0
        if (valInt == 0)
          return false;

        return true;
      }
      return false;
    }
    catch
    {
      return false;
    }
  }

  public static string GetCurrentVersion()
  {
    var thisApp = Assembly.GetExecutingAssembly();
    AssemblyName name = new AssemblyName(thisApp.FullName);
    var versionNumber = "v" + name.Version.Major + "." + name.Version.Minor;

    return versionNumber;
  }

  private static bool IsBetween(TimeSpan time, TimeSpan start, TimeSpan end)
  {
    if (start <= end)
      return time >= start && time <= end;
    else
      return false;
  }

  public static bool IsWeekend(DayOfWeek dayOfWeek)
  {
    return dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Friday;
  }

  public static int GetNumberOfWorkingDays(DateTime start, DateTime stop)
  {
    int days = 0;
    while (start <= stop)
    {
      if (!IsWeekend(start.DayOfWeek))
      {
        ++days;
      }
      start = start.AddDays(1);
    }
    return days;
  }

  public static TimeSpan GetWorkingHoursBetweenDateRange(DateTime start, DateTime end, int startWorkingHour, int EndWorkingHour)
  {
    // Will calculate the working hours within the working days 
    int totalWorkingMinutes = 0;

    for (var i = start; i < end; i = i.AddMinutes(1))
    {
      if (!IsWeekend(i.DayOfWeek))
      {
        if (i.TimeOfDay.Hours >= startWorkingHour && i.TimeOfDay.Hours < EndWorkingHour)
        {
          totalWorkingMinutes++;
        }
      }
    }

    return TimeSpan.FromMinutes(totalWorkingMinutes);
  }

  public static DateTime UpdateHolidayToWorkingDay(DateTime date, List<DateTime> upcomingHolidays)
  {
    if (IsWeekend(date.DayOfWeek) || upcomingHolidays.Contains(date.Date))
    {
      date = date.AddDays(1);
      return UpdateHolidayToWorkingDay(date, upcomingHolidays);
    }

    return date;
  }
}
