using System;
using Clarizen.Tests.Context;
using Ekin.Clarizen.Data.Request;
using Ekin.Clarizen.Tests.Models;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Xunit;

namespace Ekin.Clarizen.Tests.Steps
{
    [Binding]
    public class Api_UserSteps : BaseApiSteps
    {
        public Api_UserSteps(BaseContext context) : base(context)
        {
        }

        [Given(@"I create the following User")]
        public void GivenICreateTheFollowingUser(Table table)
        {
            var users = table.CreateSet<User>();

            foreach (var user in users)
            {
                // Give every user a different username
                user.UserName = Guid.NewGuid().ToString();
                var result = Context.Api.CreateObject("/user", user);
                Assert.True(string.IsNullOrEmpty(result.Error), result.Error);
            }

            // Allow the system to catch up
            System.Threading.Thread.Sleep(1000);
        }

        [Then(@"there are (.*) admin users")]
        public void ThenThereAreAdminUsers(int expectedAdminUserCount)
        {
           
            var query = new query("SELECT DisplayName, Admin FROM user where admin = 1");

            var results = Context.Api.ExecuteQuery(query);
            Assert.True((results.Error == null), results.Error);
            Assert.Equal((int)expectedAdminUserCount, (int)results.Data.entities.Length);
        }

        [Given(@"I delete users with an OfficePhone Number of '(.*)'")]
        public void GivenIDeleteUsersWithAnOfficePhoneNumberOf(string officePhoneNumber)
        {
            var results = TestHelper.ExecuteQuery(Context, $"SELECT FirstName,LastName, email,OfficePhone FROM user where OfficePhone = '{officePhoneNumber}'");
            foreach (var id in results.Data.GetEntityIds())
            {
                Context.Api.DeleteObject(id);
            }
        }
        [Then(@"the following users exist with an OfficePhone Number of '(.*)'")]
        public void ThenTheFollowingUsersExistWithAnOfficePhoneNumberOf(string officePhoneNumber, Table table)
        {
           var results = TestHelper.GetEntities<User>(Context, $"SELECT FirstName , LastName, email , OfficePhone ,MobilePhone ,ExternalUser ,SuperUser ,Financial  FROM user where OfficePhone = '{officePhoneNumber}'");
        }


    }
}