using System;
using System.Linq;
using Clarizen.Tests.Context;
using Ekin.Clarizen.Tests.Models;
using TechTalk.SpecFlow;
using Xunit;

namespace Ekin.Clarizen.Tests.Steps
{
    [Binding]
    public class Api_TimesheetSteps : BaseApiSteps
    {
        public Api_TimesheetSteps(BaseContext context) : base(context)
        {

        }

        [Given(@"I call MissingTimesheets for user by email '(.*)' between '(.*)' and '(.*)' inclusive")]
        public void GivenICallMissingTimesheetsForUserByEmailBetweenAnd(string userEmail, string startDate, string endDate)
        {
            System.Threading.Thread.Sleep(1000);
            var userId = TestHelper.ExecuteQuery(Context,
                                $"SELECT  email FROM user where email = '{userEmail}' and state <> \"deleted\"")
                            .Data.GetEntityIds().Single();

            var start = TestHelper.convertToDateTime(startDate);
            var end = TestHelper.convertToDateTime(endDate).AddDays(1);

            var actual = Context.Api.GetMissingTimesheets(userId,
                                                  start,
                                                  end);
            Assert.Null(actual.Error);
            Context.SUT = actual;
        }

        [Then(@"there are (.*) missing timesheets")]
        public void ThenThereAreEntities(int expected)
        {
            var actual = (Data.getMissingTimesheets)Context.SUT;
            Assert.Equal(expected,actual.Data.missingTimesheets.Count());
        }
      
        [Given(@"I get the workpattern for user '(.*)'")]
        public void GivenIGetTheWorkpatternForUser(string userEmail)
        {
            var userId = TestHelper.ExecuteQuery(Context,
                    $"SELECT  email FROM user where email = '{userEmail}' and state <> \"deleted\"")
                .Data.GetEntityIds().Single();

            var actual = Context.Api.GetCalendarInfo(userId);
            Assert.Null(actual.Error);

        }

    }
}
