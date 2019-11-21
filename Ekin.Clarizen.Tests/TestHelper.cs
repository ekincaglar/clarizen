using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Clarizen.Tests.Context;
using Clarizen.Tests.Models;
using Ekin.Clarizen.Data.Request;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Ekin.Clarizen.Tests
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


        public static List<T> ToList<T>(dynamic[] source)
        {
            var retVal = new List<T>();
            foreach (var item in source)
            {
                retVal.Add(JsonSerializer.Deserialize<T>(item.ToString()));
            }
            return retVal;
        }

        public static dynamic[] GetEntities(BaseContext context, string query)
        {
            var results = context.Api.ExecuteQuery(new query(query));
            Assert.True((results.Error == null), results.Error);
            return results.Data.entities;
        }
        internal static IEnumerable<T> GetEntities<T>(BaseContext context, string query)
        {
            return ToList<T>( GetEntities(context, query));

        }

        /// <summary>
        /// Execute the clarizen query.
        /// Throw exception if error is returned
        /// </summary>
        internal static Data.query ExecuteQuery(BaseContext context, string query)
        {
            var q = new Ekin.Clarizen.Data.Request.query(query);
            var results = context.Api.ExecuteQuery(q);
            Assert.True(string.IsNullOrEmpty(results?.Error), results?.Error);
            return results;
        }
    }
}