namespace Ekin.Clarizen.Applications.Request
{
    public class getApplicationStatus
    {
        public string applicationId { get; set; }

        public getApplicationStatus(string applicationId)
        {
            this.applicationId = applicationId;
        }
    }
}