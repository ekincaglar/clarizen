using System;
using System.Collections.Generic;
using Clarizen.Tests.Context;
using Ekin.Clarizen.Tests.Models;
using Moq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Ekin.Clarizen.Tests.Steps
{
    [Binding]
    public class TestHelperSteps : BaseApiSteps
    {
        public TestHelperSteps(BaseContext context) : base(context)
        {
        }

        [Given(@"I reset the TimeProvider")]
        public void GivenIResetTheTimeProvider()
        {
            TimeProvider.ResetToDefault();
        }

        [Given(@"I TestHelper function convertToDateTime with the following")]
        public void GivenITestHelperFunctionConvertToDateTimeWithTheFollowing(Table table)
        {
            var results = new List<TestClass1>();
            foreach (var row in table.Rows)
            {
                var value = row["Value"];
                var expected = Convert.ToDateTime(row["Result"]);
                var includeTime = row["IncludeTime"].ToString().ToLower();
                var actualDateFormat = "d MMM yyyy";
                if (includeTime == "true")
                {
                    actualDateFormat += " HH:mm:ss";
                }
                var actual = TestHelper.ConvertToDateTime(value).ToString(actualDateFormat);
                results.Add(new TestClass1() { Value = value, Result = actual, IncludeTime = row["IncludeTime"] });
            }

            table.CompareToSet(results);
        }

        [Given(@"I set the TimeProvider date to '(.*)'")]
        public void GivenWhenISetTheTimeProviderDateTo(string date)
        {
            TimeProvider.ResetToDefault();
            DateTime nowDateTime;
            DateTime todayDateTime;
            if (!date.Contains(':'))
            {
                todayDateTime = Convert.ToDateTime(date + " 00:00:00");
                nowDateTime = Convert.ToDateTime(date += " 23:14:59");
            }
            else
            {
                todayDateTime = Convert.ToDateTime(Convert.ToDateTime(date).ToString("d MMM yyyy 0:00:00"));
                nowDateTime = Convert.ToDateTime(date);
            }

            var timeMock = new Mock<TimeProvider>();
            timeMock.SetupGet(tp => tp.Now).Returns(nowDateTime);
            timeMock.SetupGet(tp => tp.Today).Returns(todayDateTime);
            TimeProvider.Current = timeMock.Object;
        }
    }
}