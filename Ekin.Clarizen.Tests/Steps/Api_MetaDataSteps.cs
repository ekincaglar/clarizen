using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ekin.Clarizen.Tests.Context;
using TechTalk.SpecFlow;
using Xunit;
using System.Xml.Linq;
using Xunit.Sdk;

namespace Ekin.Clarizen.Tests.Steps
{
    [Binding]
    public  class Api_MetaDataSteps : BaseApiSteps
    {
        public Api_MetaDataSteps(BaseContext context) : base(context)
        {
        }
        [Given(@"I call the '(.*)' entity")]
        public void GivenICallTheEntity(string entityName)
        { 
            var typeNames = new[] { entityName };
            var actual = Context.Api.DescribeEntities(typeNames);
            Assert.True(actual.Error==null,actual.Error);
            Context.SUT = actual.Data.entityDescriptions;
        }
        [Then(@"there are fields in the entity description")]
        public void ThenThereAreFieldsInTheEntityDescription()
        {
            var entity = (entityDescription[]) Context.SUT;
            var nullFields = entity.Where(a => a.fields == null)?.ToList();
            Assert.False(nullFields.Any());
        }

    }
}
