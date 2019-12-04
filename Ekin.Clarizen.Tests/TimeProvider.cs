using System;
using System.Globalization;
using System.Threading;

namespace Ekin.Clarizen.Tests
{
    public abstract class TimeProvider 
    {
        public TimeProvider()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
        }
        private static TimeProvider current = DefaultTimeProvider.Instance;

        public static TimeProvider Current
        {
            get => TimeProvider.current;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                TimeProvider.current = value;
            }
        }

        public abstract DateTime Now { get; }
        public abstract DateTime Today { get; }

        public static void ResetToDefault()
        {
            TimeProvider.current = DefaultTimeProvider.Instance;

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
        }
    }


    public class DefaultTimeProvider : TimeProvider
    {
        private static readonly DefaultTimeProvider instance =new DefaultTimeProvider();

        private DefaultTimeProvider()
        {

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
        }

        public override DateTime Now => DateTime.UtcNow;
        public override DateTime Today => DateTime.UtcNow;

        public static DefaultTimeProvider Instance => DefaultTimeProvider.instance;
    }
}