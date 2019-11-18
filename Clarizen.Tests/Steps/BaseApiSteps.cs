using Clarizen.Tests.Context;
using Ekin.Clarizen;
using Microsoft.Extensions.Configuration;
using TechTalk.SpecFlow;
using Xunit;

namespace Clarizen.Tests.Steps
{
    public abstract class BaseApiSteps
    {
        protected readonly IConfiguration Configuration;
        protected BaseContext Context;

        public BaseApiSteps(BaseContext context)
        {
            Configuration = TestHelper.GetConfiguration();
            Context = context;
        }

     
    }
}