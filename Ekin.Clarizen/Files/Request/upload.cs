namespace Ekin.Clarizen.Files.Request
{
    public class Upload
    {
        /// <summary>
        /// Id of a document to attach to
        /// </summary>
        public string DocumentId { get; set; }

        /// <summary>
        /// Additional information about the file
        /// </summary>
        public FileInformation FileInformation { get; set; }

        /// <summary>
        /// When the file is stored in Clarizen, provide the Url you received in a previous request to getUploadUrl
        /// </summary>
        public string UploadUrl { get; set; }

        public Upload(string documentId, FileInformation fileInformation, string uploadUrl)
        {
            DocumentId = documentId;
            FileInformation = fileInformation;
            UploadUrl = uploadUrl;
        }
    }
}