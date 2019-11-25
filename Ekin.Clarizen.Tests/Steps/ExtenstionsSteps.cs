using System;
using Clarizen.Tests;
using TechTalk.SpecFlow;
using Xunit;

namespace Ekin.Clarizen.Tests.Steps
{
    [Binding]
    public class ExtenstionsSteps
    {
        [Given(@"I Test Extention Method StartOfWeek with the following values")]
        public void GivenITestExtentionMethodStartOfWeekWithTheFollowingValues(Table table)
        {
            foreach (var row in table.Rows)
            {
                var target = Convert.ToDateTime(row["TargetDate"]);
                var expected = Convert.ToDateTime(row["Expected"]);
               
                #region DayOfWeek day = ... 
                DayOfWeek day;
                switch (row["DayOfWeek"].ToLower())
                {
                    case "monday":
                        day = DayOfWeek.Monday;
                        break;
                    case "tuesday":
                        day = DayOfWeek.Tuesday;
                        break;
                    case "wednesday":
                        day = DayOfWeek.Wednesday;
                        break;
                    case "thursday":
                        day = DayOfWeek.Thursday;
                        break;
                    case "friday":
                        day = DayOfWeek.Friday;
                        break;
                    case "saturday":
                        day = DayOfWeek.Saturday;
                        break;
                    case "sunday":
                        day = DayOfWeek.Sunday;
                        break;
                    default:
                    throw new ArgumentOutOfRangeException(nameof(day));
                }
                #endregion

                var actual = target.GetDayInWeek(day);
                Assert.True(expected==actual,
                    $"|{row["TargetDate"]} | {row["DayOfWeek"]} | {row["Expected"]} |\r\nActual = {actual}");
            }
        }
    }
}
