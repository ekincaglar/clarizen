namespace Ekin.Clarizen.Authentication.Request
{
    public class LoginOptions
    {
        /// <summary>
        /// (Optional) If you are a certified partner, please provide the partner id you received from Clarizen
        /// </summary>
        public string PartnerId { get; set; }
        /// <summary>
        /// A string representing your application
        /// </summary>
        public string ApplicationId { get; set; }

        public LoginOptions(string partnerId, string applicationId)
        {
            PartnerId = partnerId;
            ApplicationId = applicationId;
        }

        public LoginOptions()
        {
            PartnerId = string.Empty;
            ApplicationId = string.Empty;
        }
    }
}