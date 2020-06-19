namespace Ekin.Clarizen.Files.Request
{
    public class Download
    {
        /// <summary>
        /// Entity Id
        /// </summary>
        public string DocumentId { get; set; }

        public bool Redirect { get; set; }

        public Download(string documentId, bool redirect)
        {
            DocumentId = documentId;
            Redirect = redirect;
        }
    }
}