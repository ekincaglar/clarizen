using System;

namespace Ekin.Clarizen.POCO.Tests.Context
{
    public class BaseContext : IDisposable, IBaseContext
    {
    
        public dynamic SUT { get; set; }
        public TimeProvider TimeProvider { get; internal set; }

        public void Dispose()
        {
            TimeProvider.ResetToDefault();
        }
    }
}