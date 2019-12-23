using System;

namespace Ekin.Clarizen.POCO.Tests
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
}