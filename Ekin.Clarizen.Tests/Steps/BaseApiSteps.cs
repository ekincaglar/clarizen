using Clarizen.Tests.Context;
using Ekin.Clarizen;
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

        protected API Api => Context.Api;

        protected void DeleteTestData()
        {
            if (Context?.Api == null)
                return;
            var query = new Ekin.Clarizen.Data.Request.query(
                "SELECT name ,state FROM project where name like 'UnitTest%' and state = 'Draft'  ");

            var results = Context.Api.ExecuteQuery(query).Data;
            foreach (var projectId in results.GetEntityIds())
            {
                Context.Api.DeleteObject(projectId);
            }
        }
    }
}