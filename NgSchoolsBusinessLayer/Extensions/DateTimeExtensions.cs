using System;
using System.Globalization;

namespace NgSchoolsBusinessLayer.Extensions
{
    public static class DateTimeExtensions
    {
        public static int GetWeekNumberOfMonth(this DateTime date)
        {
            DateTime beginningOfMonth = new DateTime(date.Year, date.Month, 1);

            while (date.Date.AddDays(1).DayOfWeek != CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek)
            {
                date = date.AddDays(1);
            }

            return (int)Math.Truncate((double)date.Subtract(beginningOfMonth).TotalDays / 7f) + 1;
        }

        public static string GetMonthName(this DateTime date)
        {
            var croCulture = CultureInfo.CurrentCulture = new CultureInfo("hr-HR");
            return date.ToString("MMMM", croCulture);
        }
    }
}
