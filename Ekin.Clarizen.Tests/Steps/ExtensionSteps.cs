using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Ekin.Clarizen.Tests.Steps
{
    [Binding]
    public class ExtensionSteps
    {
        [Given(@"I check the date time extenstion GetFirstDayOfWeek")]
        public void GivenICheckTheDateTimeExtenstionGetFirstDayOfWeek(Table table)
        {
            var results = new List<ValueExpected>();
            foreach (var row in table.Rows)
            {
                var target = Convert.ToDateTime(row["Value"]);
                var actual = target.GetFirstDayOfWeek();
                results.Add(new ValueExpected() { Value = row["Value"], Expected = actual.ToString("d MMM yyyy") });
            }

            table.CompareToSet(results);
        }

        [Given(@"I Test Extention Method StartOfWeek with the following values")]
        public void GivenITestExtenstionMethodStartOfWeekWithTheFollowingValues(Table table)
        {
            var results = new List<StartOfWeekData>(); ;
            foreach (var row in table.Rows)
            {
                var target = Convert.ToDateTime(row["TargetDate"]);
                var expected = Convert.ToDateTime(row["Expected"]);
               

                var result = new StartOfWeekData()
                {
                    TargetDate = target.ToString("dd MMM yyyy"),
                    DayOfWeek = row["DayOfWeek"],
                    Expected = expected.ToString("dd MMM yyyy")
                };

                results.Add(result);
            }
            table.CompareToSet(results);
        }

        [Then(@"I check extenstion method GetDayInWeek returns the following")]
        public void ThenICheckExtenstionMethodGetDayInWeekReturnsTheFollowing(Table table)
        {
            var results = new List<ValueExpected>();
            foreach (var row in table.Rows)
            {
                var dow = getDayOfWeek(row["Value"]);
                var actual = TimeProvider.Current.Now.GetDayInWeek(dow).ToString("d MMM yyyy");
                results.Add(new ValueExpected() { Value = row["Value"], Expected = actual });
            }

            table.CompareToSet(results);
        }

        private DayOfWeek getDayOfWeek(string dayOfWeek)
        {
            DayOfWeek day;
            switch (dayOfWeek.ToLower())
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

            return day;
        }

        public struct StartOfWeekData
        {
            public string DayOfWeek { get; set; }
            public string Expected { get; set; }
            public string TargetDate { get; set; }
        }

        public struct ValueExpected
        {
            public string Expected { get; set; }
            public string Value { get; set; }
        }
    }
}