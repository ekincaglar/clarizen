using Clarizen.Tests.Context;
using Microsoft.Extensions.Configuration;

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