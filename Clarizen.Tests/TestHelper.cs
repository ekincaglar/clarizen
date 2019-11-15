using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.Logging;

namespace Clarizen.Tests
{
    internal class TestHelper
    {
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

        internal static ILogger<T> GetILoggerFactory<T>()
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddConsole()
                    .AddEventLog();
            });

            return loggerFactory.CreateLogger<T>();
        }
    }
}
