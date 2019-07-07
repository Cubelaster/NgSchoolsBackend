using System;
using System.Globalization;

namespace NgSchoolsBusinessLayer.Extensions
{
    public static class DateTimeExtensions
    {
        public static int GetWeekNumberOfMonth(this DateTime date)
        {
            DateTime firstMonthDay = new DateTime(date.Year, date.Month, 1);
            DateTime firstMonthMonday = firstMonthDay.AddDays((DayOfWeek.Monday + 7 - firstMonthDay.DayOfWeek) % 7);

            var weekBuffer = firstMonthMonday > firstMonthDay ? 1 : 0;

            var weekNumber = Math.Ceiling((decimal)(date - firstMonthMonday).Days / 7) + weekBuffer;
            return (int)weekNumber;
        }

        public static string GetMonthName(this DateTime date)
        {
            var croCulture = CultureInfo.CurrentCulture = new CultureInfo("hr-HR");
            return date.ToString("MMMM", croCulture);
        }
    }
}
