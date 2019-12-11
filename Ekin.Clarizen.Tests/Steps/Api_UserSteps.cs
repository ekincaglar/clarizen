using System;
using System.Linq;
using Ekin.Clarizen.Data.Request;
using Ekin.Clarizen.Tests.Context;
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

        //////[Given(@"I change the state to '(.*)' for user by email '(.*)'")]
        //////public void GivenIChangeTheStateToForUserByEmail(string newState, string email)
        //////{
        //////    var result = TestHelper.ExecuteQuery(Context, $"SELECT FirstName,LastName, email,OfficePhone,state FROM user where email = '{email}' and state <> \"deleted\"");
        //////    Assert.True(result.Data.entities.Length == 1, $"There should only be one user found,  {result.Data.entities.Length} users where found.");
            
        //////    Context.SUT = result;

        //////   var changeStateResult = Context.Api.ChangeState(result.Data.GetEntityIds(), newState);
        //////   Assert.True(string.IsNullOrEmpty(changeStateResult?.Error),changeStateResult?.Error);
          
        //////}

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

        [Given(@"I set the test user to be '(.*)'")]
        public void GivenISetTheTestUserToBe(string email)
        {
            var result = TestHelper.ExecuteQuery(Context, $"SELECT FirstName,state FROM user where email = '{email}' and state <> \"deleted\"");
            Context.UserId = result.Data.GetEntityIds().First();
        }

        [Given(@"I delete users with an OfficePhone Number of '(.*)'")]
        public void GivenIDeleteUsersWithAnOfficePhoneNumberOf(string officePhoneNumber)
        {
            var results = TestHelper.ExecuteQuery(Context
                    , $"SELECT FirstName FROM user where OfficePhone = '{officePhoneNumber}' and state <> \"deleted\"");
            foreach (var id in results.Data.GetEntityIds())
            {
                var result = Context.Api.DeleteObject(id);
                if (result.Error != null)
                {
                    throw new Exception(result.Error);
                }

            }
        }

        [Then(@"the following users exist with an OfficePhone Number of '(.*)'")]
        public void ThenTheFollowingUsersExistWithAnOfficePhoneNumberOf(string officePhoneNumber, Table table)
        {
            var results = TestHelper.GetEntities<User>(Context, $"SELECT FirstName , LastName, email , OfficePhone ,MobilePhone ,ExternalUser ,SuperUser ,Financial  FROM user where OfficePhone = '{officePhoneNumber}'");
        }

        [Then(@"there are (.*) admin users")]
        public void ThenThereAreAdminUsers(int expectedAdminUserCount)
        {
            var query = new query("SELECT DisplayName, Admin FROM user where admin = 1");

            var results = Context.Api.ExecuteQuery(query);
            Assert.True((results.Error == null), results.Error);
            Assert.Equal((int)expectedAdminUserCount, (int)results.Data.entities.Length);
        }
    }
}