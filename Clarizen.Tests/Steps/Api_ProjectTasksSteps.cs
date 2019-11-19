using System;
using Clarizen.Tests.Context;
using Clarizen.Tests.Models;
using TechTalk.SpecFlow;

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
    }
}