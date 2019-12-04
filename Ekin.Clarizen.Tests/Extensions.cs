using System;
using System.Globalization;
using System.Threading;
using Ekin.Clarizen.Tests;

namespace Clarizen.Tests
{
    public static class Extensions
    {
        public static DateTime GetDayInWeek(this DateTime dt, DayOfWeek dayOfWeek)
        {

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");

            var firstDayOfWeekCorrection = 0;
            var firstDayOfWeek = Thread.CurrentThread.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            if (firstDayOfWeek == DayOfWeek.Sunday)
            {
                // used to correct the USA insistence of sunday being first day of the week.
                firstDayOfWeekCorrection += 1;
            }

            var firstDateOfWeek = dt.GetFirstDayOfWeek().AddDays(firstDayOfWeekCorrection);
            return firstDateOfWeek.AddDays(dayOfWeek.ToInt());
        }

        /// <summary>
        /// Returns the first day of the week that the specified date is in.
        /// </summary>
        public static DateTime GetFirstDayOfWeek(this DateTime dt)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
            var firstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            DateTime firstDayInWeek = dt.Date;
            while (firstDayInWeek.DayOfWeek != firstDayOfWeek)
                firstDayInWeek = firstDayInWeek.AddDays(-1);

            return firstDayInWeek;
        }

        public static int ToInt(this DayOfWeek dow)
        {
            switch (dow)
            {
                case DayOfWeek.Monday:
                    return 0;

                case DayOfWeek.Tuesday:
                    return 1;

                case DayOfWeek.Wednesday:
                    return 2;

                case DayOfWeek.Thursday:
                    return 3;

                case DayOfWeek.Friday:
                    return 4;

                case DayOfWeek.Saturday:
                    return 5;

                case DayOfWeek.Sunday:
                    return 6;

                default:
                    throw new ArgumentOutOfRangeException(nameof(dow), dow, null);
            }
        }
    }
}