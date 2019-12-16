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
    public  class Api_ExpensesSteps : BaseApiSteps
    {
        public Api_ExpensesSteps(BaseContext context) : base(context)
        {
        }


    }
}
