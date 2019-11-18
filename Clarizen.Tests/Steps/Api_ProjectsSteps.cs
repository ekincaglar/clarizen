using System;
using System.Collections.Generic;
using System.Linq;
using Clarizen.Tests.Context;
using Clarizen.Tests.Models;
using TechTalk.SpecFlow;
using Xunit;

namespace Clarizen.Tests.Steps
{
    [Binding]
    public class ProjectsSteps : BaseApiSteps,IDisposable
    {
        public ProjectsSteps(BaseContext context) : base(context)
        {
            CancelUnitTestProjects();
        }

        [Given(@"I create a project\.")]
        public void GivenICreateAProject()
        {
            Context.Api.isSandbox = true;

            var model = new Project(true)
            {
                Name = $"UnitTest {Guid.NewGuid().ToString()}",
                Description = "SpecFlow",
                StartDate = DateTime.Today.AddDays(-1),
                DueDate = DateTime.Now
                
            };
            var actual = Context.Api.CreateObject("/Project", model);
            Assert.Null(actual.Error);
            Context.ProjectId = actual.Data.id;
        }

        [Then(@"there are '(.*)' projects")]
        public void ThenThereAreProjects(int expectedProjectCount)
        {
            var query = new Ekin.Clarizen.Data.Request.query("SELECT name, state FROM project where state = 'Active'");

            var results = Context.Api.ExecuteQuery(query);
            Assert.True((results.Error == null), results.Error);
            Assert.Equal(expectedProjectCount, (int)results.Data.entities.Length);
        }

        public void Dispose()
        {
            CancelUnitTestProjects();
        }

        private void CancelUnitTestProjects()
        {

            var queryState = new Ekin.Clarizen.Data.Request.query("SELECT name  FROM state where name = 'Cancelled'");
            var resultsState = Context.Api.ExecuteQuery(queryState).Data;
            var query = new Ekin.Clarizen.Data.Request.query(
                "SELECT name ,state FROM project where name like 'UnitTest%' and state = 'Draft'  ");

            var results = Context.Api.ExecuteQuery(query).Data;
            var cancelledId = resultsState.GetEntityIds().First().Split('/').Last();
            /////Change Project to
            foreach (var projectId in results.GetEntityIds())
            {
                
                var changeStateResult=Context.Api.ChangeState(
                new string[]{projectId},
                cancelledId);
            }
        }
    }
}