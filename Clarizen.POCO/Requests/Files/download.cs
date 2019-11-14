namespace Ekin.Clarizen.Files.Request
{
    public class download
    {
        /// <summary>
        /// Entity Id
        /// </summary>
        public string documentId { get; set; }

        public bool redirect { get; set; }

        public download(string documentId, bool redirect)
        {
            this.documentId = documentId;
            this.redirect = redirect;
        }
    }
}