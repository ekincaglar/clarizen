namespace Ekin.Clarizen.Authentication.Result
{
    public class GetServerDefinition
    {
        /// <summary>
        /// The actual API url to use for subsequent calls
        /// </summary>
        public string ServerLocation { get; set; }

        public string AppLocation { get; set; }

        public int OrganizationId { get; set; }

        public GetServerDefinition()
        {

        }
    }
}