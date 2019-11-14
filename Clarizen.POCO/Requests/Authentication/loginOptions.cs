namespace Ekin.Clarizen.Authentication.Request
{
    public class loginOptions
    {
        /// <summary>
        /// (Optional) If you are a certified partner, please provide the partner id you received from Clarizen
        /// </summary>
        public string partnerId { get; set; }

        /// <summary>
        /// A string representing your application
        /// </summary>
        public string applicationId { get; set; }

        public loginOptions(string partnerId, string applicationId)
        {
            this.partnerId = partnerId;
            this.applicationId = applicationId;
        }

        public loginOptions()
        {
            this.partnerId = string.Empty;
            this.applicationId = string.Empty;
        }
    }
}