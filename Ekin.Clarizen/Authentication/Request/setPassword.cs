namespace Ekin.Clarizen.Authentication.Request
{
    public class SetPassword
    {
        /// <summary>
        /// Fully qualified Id of the user, e.g. /Organization/SomeOrgId/User/SomeUserId
        /// </summary>
        public string UserId;

        /// <summary>
        /// The password
        /// </summary>
        public string NewPassword;

        public SetPassword(string userId, string newPassword)
        {
            UserId = userId;
            NewPassword = newPassword;
        }
    }
}