using System.Globalization;
using Ekin.Clarizen.POCO.Tests.Context;
using Microsoft.Extensions.Configuration;

namespace Ekin.Clarizen.POCO.Tests.Steps
{
    public abstract class BaseApiSteps
    {
        protected readonly IConfiguration Configuration;
        protected BaseContext Context;

        public BaseApiSteps(BaseContext context)
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-GB");
            Configuration = TestHelper.GetConfiguration();
            Context = context;
        }

    }
}