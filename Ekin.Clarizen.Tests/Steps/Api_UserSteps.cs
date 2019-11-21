﻿using System;
using Clarizen.Tests.Context;
using Ekin.Clarizen.Data.Request;
using TechTalk.SpecFlow;
using Xunit;

namespace Clarizen.Tests.Steps
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
            foreach (var row in table.Rows)
            {
                throw new NotImplementedException();
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
    }
}