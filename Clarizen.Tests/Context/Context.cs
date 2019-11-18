using Clarizen.Tests.Models;
using Ekin.Clarizen;
using Ekin.Clarizen.Data;

namespace Clarizen.Tests.Context
{
    public class BaseContext
    {
        public API Api { get; set; }
        public string ProjectId { get; internal set; }
    }
}