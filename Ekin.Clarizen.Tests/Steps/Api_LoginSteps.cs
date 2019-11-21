using Clarizen.Tests.Context;
using Ekin.Clarizen;
using TechTalk.SpecFlow;
using Xunit;

namespace Ekin.Clarizen.Tests.Steps
{
    [Binding]
    public class Api_LoginSteps : BaseApiSteps
    {
        public Api_LoginSteps(BaseContext context) : base(context)
        {
        }

        [Given(@"I Login using login '(.*)' and password '(.*)'")]
        public void GivenILoginUsingLoginAndPassword(string username, string password)
        {
            var target = new API();
            var actual = target.Login(username, password);
            Assert.False(actual, $"You should not be able to login using credentials uid= '{username}' and pwd= '{password}'");
        }

        [Given(@"I remove pre-existing test data")]
        public void GivenIRemovePre_ExistingTestData()
        {
            base.DeleteTestData();
        }

        [Given(@"I Login using credentials in appsettings")]
        protected void GivenILoginUsingCredentialsInAppsettings()
        {
            var username = Configuration["Clarizen:Credentials:UserName"];
            var password = Configuration["Clarizen:Credentials:Password"];
            var target = new API();
            var actual = target.Login(username, password);
            Assert.True(actual, "Could not login using credentials in appsettings.json");
            Context.Api = target;
        }
    }
}