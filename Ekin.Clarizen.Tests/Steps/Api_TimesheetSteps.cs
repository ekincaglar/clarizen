using System;
using System.Linq;
using Clarizen.Tests.Context;
using Ekin.Clarizen.Tests.Models;
using TechTalk.SpecFlow;

namespace Ekin.Clarizen.Tests.Steps
{
    [Binding]
    public class Api_TimesheetSteps : BaseApiSteps
    {
        public Api_TimesheetSteps(BaseContext context) : base(context)
        {

        }

        [Given(@"I call MissingTimesheets for user by email '(.*)' between '(.*)' and '(.*)'")]
        public void GivenICallMissingTimesheetsForUserByEmailBetweenAnd(string userEmail, string startDate, string endDate)
        {
            System.Threading.Thread.Sleep(1000);
            var userId = TestHelper.ExecuteQuery(Context, $"SELECT  email FROM user where email = '{userEmail}' and state <> \"deleted\"").Data.GetEntityIds().Single();

            var start = TestHelper.convertToDateTime(startDate);
            var end = TestHelper.convertToDateTime(endDate);

            Context.SUT = Context.Api.GetMissingTimesheets(userId,
                                                  start,
                                                  end);
        }
    }
}
