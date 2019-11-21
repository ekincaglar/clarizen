using System;
using System.Diagnostics;
using Clarizen.Tests.Context;
using Clarizen.Tests.Models;
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

        [Given(@"I create a project\.")]
        public void GivenICreateAProject()
        {
            var model = new Project(true)
            {
                Name = $"UnitTest {Guid.NewGuid().ToString()}",
                Description = "SpecFlow Test Data",
                StartDate = DateTime.Today.AddDays(-1),
                DueDate = DateTime.Now
            };
            var actual = Context.Api.CreateObject("/Project", model);
            Assert.Null(actual.Error);
            Context.ProjectId = actual.Data.id;
            Debug.WriteLine($"~~~~~ ProjectId || {Context.ProjectId}");
            Debug.WriteLine($"~~~~~      Name || {model.Name}");
        }

        [Then(@"there are '(.*)' projects")]
        public void ThenThereAreProjects(int expectedProjectCount)
        {
            var query = new Ekin.Clarizen.Data.Request.query("SELECT name, state FROM project where state = 'Active'");

            var results = Context.Api.ExecuteQuery(query);
            Assert.True((results.Error == null), results.Error);
            Assert.Equal(expectedProjectCount, (int)results.Data.entities.Length);
        }
    }
}