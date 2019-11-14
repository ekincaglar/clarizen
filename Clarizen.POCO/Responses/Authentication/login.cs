namespace Ekin.Clarizen.Authentication.Result
{
    public class login
    {
        /// <summary>
        /// A unique ID representing the current session
        /// </summary>
        public string sessionId { get; set; }

        /// <summary>
        /// The unique ID of the current user. Can be used to retrieve additional information about the current user
        /// </summary>
        public string userId { get; set; }

        /// <summary>
        /// The unique ID of the current organization. Can be used to retrieve additional information about the current organization
        /// </summary>
        public string organizationId { get; set; }

        /// <summary>
        /// Indicates which license the current user is assigned
        /// Possible values: Full | Limited | Email | None | TeamMember | Social
        /// </summary>
        public string licenseType { get; set; }
    }
}