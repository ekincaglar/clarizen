using System;
using System.Globalization;
using System.Security.Cryptography;
using Ekin.Clarizen;

namespace Clarizen.Tests
{
    public static class Extensions
    {
        public static DateTime GetDayInWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            var firstDayOfWeek = dt.GetFirstDayOfWeek();
            return dt.AddDays(startOfWeek.ToInt()-1);
        }
        public static  int ToInt(this DayOfWeek dow)
        {
            switch (dow)
            {
                case DayOfWeek.Monday:
                    return 1;
                    break;
                case DayOfWeek.Tuesday:
                    return 2;
                    break;
                case DayOfWeek.Wednesday:
                    return 3;
                    break;
                case DayOfWeek.Thursday:
                    return 4;
                case DayOfWeek.Friday:
                    return 5;
                    break;
                case DayOfWeek.Saturday:
                    return 6;
                    break;
                case DayOfWeek.Sunday:
                    return 7;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dow), dow, null);
                    break;
            }

        }
        ///////////////// <summary>
        ///////////////// Returns the first day of the week that the specified
        ///////////////// date is in using the current culture. 
        ///////////////// </summary>
        //////////////public static DateTime GetFirstDayOfWeek(DateTime dayInWeek)
        //////////////{
        //////////////    var defaultCultureInfo = CultureInfo.CurrentCulture;
        //////////////    return GetFirstDateOfWeek(dayInWeek, defaultCultureInfo);
        //////////////}

        /// <summary>
        /// Returns the first day of the week that the specified date 
        /// is in. 
        /// </summary>
        public static DateTime GetFirstDayOfWeek(this DateTime dt )
        {
            
            DateTime firstDayInWeek = dt.Date;
            while (firstDayInWeek.DayOfWeek != DayOfWeek.Monday)
                firstDayInWeek = firstDayInWeek.AddDays(-1);

            return firstDayInWeek;
        }
    }
}