using System.Linq;
using Clarizen.Tests.Context;
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

        [Given(@"I call MissingTimesheets for testuser between '(.*)' and '(.*)' inclusive")]
        public void GivenICallMissingTimesheetsForTestuserBetweenAndInclusive(string startDate, string endDate)
        {
            GetMissingTimeSheets(startDate, endDate, Context.UserId);
        }

        [Given(@"I get the workpattern for the test user")]
        public void GivenIGetTheWorkpatternForTheTestUser()
        {
            var actual = Context.Api.GetCalendarInfo(Context.UserId);
            Assert.Null(actual.Error);
        }

        [Then(@"there are (.*) missing timesheets")]
        public void ThenThereAreEntities(int expected)
        {
            var actual = (Data.getMissingTimesheets)Context.SUT;
            Assert.Equal(expected, actual.Data.missingTimesheets.Count());
        }

        private void GetMissingTimeSheets(string startDate, string endDate, string userId)
        {
            var start = TestHelper.convertToDateTime(startDate);
            var end = TestHelper.convertToDateTime(endDate).AddDays(1);

            var actual = Context.Api.GetMissingTimesheets(userId,
                start,
                end);
            Assert.Null(actual.Error);
            Context.SUT = actual;
        }
    }
}