namespace KH.BuildingBlocks.Apis.Extentions;

public static class DateTimeExtensions
{
  private static bool IsWeekend(DayOfWeek dayOfWeek)
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

  public static TimeSpan GetWorkingHoursBetweenDateRange(DateTime start, DateTime end, int startWorkingHour, int endWorkingHour)
  {
    int totalWorkingMinutes = 0;

    for (var i = start; i < end; i = i.AddMinutes(1))
    {
      if (!IsWeekend(i.DayOfWeek))
      {
        if (i.TimeOfDay.Hours >= startWorkingHour && i.TimeOfDay.Hours < endWorkingHour)
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
