using System;
using System.Collections.Generic;
using System.Linq;
using Clarizen.Tests.Context;
using Clarizen.Tests.Models;
using Ekin.Clarizen.Data.Request;
using TechTalk.SpecFlow;
using Xunit;
using System.Text.Json;
using TechTalk.SpecFlow.Assist;

namespace Clarizen.Tests.Steps
{
    [Binding]
    public class Api_ProjectTasksSteps : BaseApiSteps
    {
        public Api_ProjectTasksSteps(BaseContext context) : base(context)
        {
        }

        [Given(@"I add task '(.*)' for today only")]
        public void GivenIAddTaskForTodayOnly(string taskName)
        {
            var task = new Task(taskName)
            {
                Parent = Context.ProjectId,
                StartDate = DateTime.Today,
                DueDate = DateTime.Now
            };
            var putObject = Api.CreateObject("/Task", task);

            if (putObject?.Error != null)
            {
                throw new Exception(putObject?.Error);
            }
        }

        [Given(@"I add the following tasks to the project")]
        public void GivenIAddTheFollowingTasksToTheProject(Table table)
        {
            foreach (var row in table.Rows)
            {
                GivenIAddTaskForTodayOnly(row["name"]);
            }
        }

        [Then(@"the following tasks exist in the project")]
        public void ThenTheFollowingTasksExistInTheProject(Table table)
        {
            var q = new query($"Select name from task where Project = '{Context.ProjectId}'");
            var results = Context.Api.ExecuteQuery(q);
            Assert.True((results.Error == null), results.Error);
            var actual = TestHelper.ToList<ClarizenEntity>(results.Data.entities);
           table.CompareToSet(actual);
        }
        [Given(@"I wait (.*) second")]
        public void GivenIWaitSecond(int waitInSeconds)
        {
            System.Threading.Thread.Sleep(waitInSeconds * 1000);
        }

    }
}
