using Clarizen.Tests.Context;
using Ekin.Clarizen.Data.Request;
using TechTalk.SpecFlow;
using Xunit;

namespace Clarizen.Tests.Steps
{
    [Binding]
    public class ProjectsSteps : BaseApiSteps
    {
        public ProjectsSteps(BaseContext context) : base(context)
        {
        }

        [Then(@"there are '(.*)' projects")]
        public void ThenThereAreProjects(int expectedProjectCount)
        {
            var query = new query("SELECT name, state FROM project where state = 'Active'");

            var results = Context.Api.ExecuteQuery(query);
            Assert.True((results.Error == null), results.Error);
            Assert.Equal(expectedProjectCount, results.Data.entities.Length);
        }
    }
}