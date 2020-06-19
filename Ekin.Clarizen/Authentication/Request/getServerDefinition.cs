namespace Ekin.Clarizen.Authentication.Request
{
    public class GetServerDefinition
    {
        /// <summary>
        /// The user name to authenticate with
        /// </summary>
        public string UserName;

        /// <summary>
        /// The password
        /// </summary>
        public string Password;

        /// <summary>
        /// Additional information required during the login process
        /// </summary>
        public LoginOptions LoginOptions;

        public GetServerDefinition(string userName, string password, LoginOptions loginOptions)
        {
            UserName = userName;
            Password = password;
            LoginOptions = loginOptions;
        }
    }
}