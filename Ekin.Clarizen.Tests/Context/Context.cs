using System;
using Ekin.Clarizen;
using Ekin.Clarizen.Data;

namespace Clarizen.Tests.Context
{
    public class BaseContext:IDisposable
    {
        public API Api { get; set; }
        public string ProjectId { get; internal set; }
        public dynamic SUT { get; internal set; }
        public string UserId { get; set; }

        public void Dispose()
        {
            Api?.Logout();
        }
    }
}