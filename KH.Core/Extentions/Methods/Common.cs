namespace KH.Helper.Extentions.Methods
{
  public static class Common
  {
    public static string TrimValue(this string val)
    {
      val = string.IsNullOrEmpty(val) || string.IsNullOrWhiteSpace(val) ? null : val.Trim();

      if (string.IsNullOrEmpty(val) || val == "null" || val == "undefined")
        return null;

      return val;
    }

    public static bool IsNotEmpty(this string val)
    {
      try
      {
        if (string.IsNullOrEmpty(val.TrimValue()))
          return false;

        return true;
      }
      catch
      {

        return false;
      }
    }

    public static bool IsNotEmptyNumber(this object val)
    {
      try
      {
        if (val is null)
          return false;

        var valStr = val.ToString();

        if (!valStr.IsNotEmpty())
          return false;

        int valint = int.Parse(valStr);

        if (valint == 0)
          return false;

        return true;
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
      if (dayOfWeek == DayOfWeek.Saturday
          || dayOfWeek == DayOfWeek.Friday)
      {
        //Console.WriteLine("Weekend");
        return true;
      }
      else
      {
        return false;
      }
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
      //will calculate the working hours within the working days 

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

      //var totalMinutes = 12534;
      //var time = TimeSpan.FromMinutes(totalMinutes);
      //TimeSpan timeSpan = TimeSpan.FromHours(totalWorkingMinutes);
      TimeSpan timeSpan = TimeSpan.FromMinutes(totalWorkingMinutes);

      return timeSpan;
    }

    public static DateTime UpdateHolidayToWorkingDay(DateTime date, List<DateTime> upcomingHolidays)
    {
      if (IsWeekend(date.DayOfWeek) || upcomingHolidays.Count > 0 && upcomingHolidays.Contains(date.Date))
      {
        date = date.AddDays(1);
        UpdateHolidayToWorkingDay(date, upcomingHolidays);
      }
      else
      {
        return date;
      }

      return date;

    }
  }
}
