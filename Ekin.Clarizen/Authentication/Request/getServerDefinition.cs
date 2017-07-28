using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen.Authentication.Request
{
    public class getServerDefinition
    {
        /// <summary>
        /// The user name to authenticate with
        /// </summary>
        public string userName;
        /// <summary>
        /// The password
        /// </summary>
        public string password;
        /// <summary>
        /// Additional information required during the login process
        /// </summary>
        public loginOptions loginOptions;

        public getServerDefinition(string userName, string password, loginOptions loginOptions)
        {
            this.userName = userName;
            this.password = password;
            this.loginOptions = loginOptions;
        }
    }
}