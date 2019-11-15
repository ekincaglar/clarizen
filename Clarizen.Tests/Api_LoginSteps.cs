using System;
using Ekin.Clarizen;
using Microsoft.Extensions.Configuration;
using TechTalk.SpecFlow;
using Xunit;

namespace Clarizen.Tests
{
    [Binding]
    public class Api_LoginSteps
    {
        private readonly IConfiguration _configuration;

        public Api_LoginSteps()
        {
            _configuration = TestHelper.GetConfigurationRoot();
        }
        [Given(@"I Login using credentials in appsettings")]
        public void GivenILoginUsingCredentialsInAppsettings()
        {
            var username = _configuration["Clarizen:Credentials:UserName"];
            var password = _configuration["Clarizen:Credentials:Password"];
            var target = new API();
            var actual = target.Login(username, password);
            Assert.True(actual,"Could not login using credentials in appsettings.json");
        }
    }
}
