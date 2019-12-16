using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ekin.Clarizen.Tests.Context;
using TechTalk.SpecFlow;

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
            throw new NotImplementedException();
        }

    }
}
