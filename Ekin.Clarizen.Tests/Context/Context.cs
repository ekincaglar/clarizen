using System;

namespace Ekin.Clarizen.Tests.Context
{
    public class BaseContext : IDisposable, IBaseContext
    {
        public BaseContext()
        {
            TimeProvider.ResetToDefault();
        }

        public API Api { get; set; }
        public string ProjectId { get;  set; }
        public dynamic SUT { get;  set; }
        public string UserId { get; set; }

        public void Dispose()
        {
            TimeProvider.ResetToDefault();
            Api?.Logout();
        }
    }

    public interface IBaseContext
    {
        string ProjectId { get;  set; }
        API Api { get; set; }
        dynamic SUT { get;  set; }
        string UserId { get; set; }
    }
}