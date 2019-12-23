using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace Ekin.Clarizen.POCO.Tests
{
    internal class TestHelper
    {
        public static List<T> ToList<T>(dynamic[] source)
        {
            return source.Select(item => JsonSerializer.Deserialize<T>(item.ToString())).Cast<T>().ToList();
        }

        internal static DateTime ConvertToDateTime(string value)
        {
            var retVal = (value.ToLower()) switch
            {
                "<<today>>" => TimeProvider.Current.Today,
                "<<yesterday>>" => TimeProvider.Current.Today.AddDays(-1),
                "<<now>>" => TimeProvider.Current.Now,
                "<<yearstart>>" => new DateTime(TimeProvider.Current.Now.Year, 1, 1),
                "<<monthstart>>" => new DateTime(TimeProvider.Current.Now.Year, TimeProvider.Current.Now.Month, 1),
                "<<mondaylastweek>>" => TimeProvider.Current.Today.AddDays(-7).GetDayInWeek(DayOfWeek.Monday),
                "<<fridaylastweek>>" => TimeProvider.Current.Today.AddDays(-7).GetDayInWeek(DayOfWeek.Friday),
                "<<tomorrow>>" => TimeProvider.Current.Today.AddDays(1),
                "<<mondaynextweek>>" => TimeProvider.Current.Today.AddDays(7).GetDayInWeek(DayOfWeek.Monday),
                "<<fridaynextweek>>" => TimeProvider.Current.Today.AddDays(7).GetDayInWeek(DayOfWeek.Friday),
                _ => Convert.ToDateTime(value),
            };
            if (retVal.Year == 1)
            {
                throw new ArgumentOutOfRangeException(value, $"parameter '{nameof(value)}' value '{value}' is out of range");
            }
            return retVal;
        }

        internal static IConfiguration GetConfiguration(string settingsFileName = "appsettings.json")
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), settingsFileName);
            if (!File.Exists(path))
            {
                throw new FileNotFoundException(path);
            }

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(path, optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

#if DEBUG
            path = Path.Combine(Directory.GetCurrentDirectory(), "secrets.json");
            if (File.Exists(path))
            {
                builder.AddJsonFile(path, optional: false, reloadOnChange: true);
            }
#endif
            return builder.Build();
        }
    }
}