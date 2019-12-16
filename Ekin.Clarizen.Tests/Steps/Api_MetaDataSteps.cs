using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ekin.Clarizen.Tests.Context;
using TechTalk.SpecFlow;
using Xunit;
using System.Xml.Linq;
namespace Ekin.Clarizen.Tests.Steps
{
    [Binding]
    public  class Api_MetaDataSteps : BaseApiSteps
    {
        public Api_MetaDataSteps(BaseContext context) : base(context)
        {
        }

        [Given(@"I call the weekdays entity")]
        public void GivenICallTheWeekdaysEntity()
        {
            var typeNames = new[] { "WeekDays","BaseFile","User","Organization","TimeSheet","Bug" };
            var actual = Context.Api.DescribeMetadata(typeNames);
            var nullFields = actual.Data.entityDescriptions.Where(a => a.fields == null)?.ToList();
            Assert.False(nullFields.Any());
        }

    }
}
