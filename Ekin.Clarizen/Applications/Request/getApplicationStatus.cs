namespace Ekin.Clarizen.Applications.Request
{
    public class GetApplicationStatus
    {
        public string ApplicationId { get; set; }

        public GetApplicationStatus(string applicationId)
        {
            this.ApplicationId = applicationId;
        }
    }
}