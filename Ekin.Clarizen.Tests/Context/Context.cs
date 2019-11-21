using System;
using Ekin.Clarizen;

namespace Clarizen.Tests.Context
{
    public class BaseContext:IDisposable
    {
        public API Api { get; set; }
        public string ProjectId { get; internal set; }
        public void Dispose()
        {
            Api?.Logout();
        }
    }
}