
using Clarizen.Tests.Context;
using Ekin.Clarizen.Data.Request;
using TechTalk.SpecFlow;
using Xunit;
using System;
using System.Collections.Generic;
using System.Text;
using Ekin.Clarizen;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace Clarizen.Tests.Steps
{
    [Binding]
    public class ProjectsSteps:BaseApiSteps
    {

        public ProjectsSteps(BaseContext context):base (context)
        {
        }
   
        [Then(@"there are '(.*)' projects")]
        public void ThenThereAreProjects(int expectedProjectCount)
        {
            var query = new query("SELECT name, state FROM project where state = 'Active'");

            var results = Context.Api.ExecuteQuery(query);
            Assert.True((results.Error == null), results.Error);
            Assert.True(results.Data.entities.Length==expectedProjectCount);
        }
    }
}
