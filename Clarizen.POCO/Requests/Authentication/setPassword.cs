namespace Ekin.Clarizen.Authentication.Request
{
    public class setPassword
    {
        /// <summary>
        /// Fully qualified Id of the user, e.g. /Organization/SomeOrgId/User/SomeUserId
        /// </summary>
        public string userId;

        /// <summary>
        /// The password
        /// </summary>
        public string newPassword;

        public setPassword(string userId, string newPassword)
        {
            this.userId = userId;
            this.newPassword = newPassword;
        }
    }
}