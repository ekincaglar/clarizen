using System.Globalization;
using Ekin.Clarizen.Tests.Context;
using Microsoft.Extensions.Configuration;

namespace Ekin.Clarizen.Tests.Steps
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

        protected API Api => Context.Api;

        protected void DeleteTestData()
        {
            TimeProvider.ResetToDefault();

            DeleteQuery("select name, state  from user where state <> 'deleted' and name like '%unittest%'", Context.Api);
            DeleteQuery("SELECT name ,state FROM project where name like 'UnitTest%'",Context.Api);
        }

        protected void DeleteQuery(string czql, API api = null)
        {
            var query = new Ekin.Clarizen.Data.Request.query(czql);
            if (api == null)
            {
                api = new API();
                var username = Configuration["Clarizen:Credentials:UserName"];
                var password = Configuration["Clarizen:Credentials:Password"];
                api.Login(username, password);
            }
            var results = api.ExecuteQuery(query).Data;
            if (results == null)
            {
                return;
            }

            foreach (var projectId in results.GetEntityIds())
            {
                api.DeleteObject(projectId);
            }
        }
    }
}