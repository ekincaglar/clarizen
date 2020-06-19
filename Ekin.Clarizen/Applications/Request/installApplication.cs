namespace Ekin.Clarizen.Applications.Request
{
    public class InstallApplication
    {
        public string ApplicationId { get; set; }
        public bool AutoEnable { get; set; }

        public InstallApplication(string applicationId, bool autoEnable)
        {
            ApplicationId = applicationId;
            AutoEnable = autoEnable;
        }
    }
}