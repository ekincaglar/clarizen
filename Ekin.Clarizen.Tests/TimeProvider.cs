using System;
using System.Globalization;

namespace Ekin.Clarizen.Tests
{
    public class DefaultTimeProvider : TimeProvider
    {
        private static readonly DefaultTimeProvider instance = new DefaultTimeProvider();

        private DefaultTimeProvider()
        {
        }

        public static DefaultTimeProvider Instance => DefaultTimeProvider.instance;
        public override DateTime Now => DateTime.UtcNow;
        public override DateTime Today => DateTime.UtcNow;
    }

    public abstract class TimeProvider
    {
        private static TimeProvider current = DefaultTimeProvider.Instance;


        public static TimeProvider Current
        {
            get => TimeProvider.current;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                TimeProvider.current = value;
            }
        }

        public abstract DateTime Now { get; }
        public abstract DateTime Today { get; }

        public static void ResetToDefault()
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-GB");
            TimeProvider.current = DefaultTimeProvider.Instance;
        }
    }
}