namespace Ekin.Clarizen.Files.Request
{
    public class updateImage
    {
        /// <summary>
        /// Id of an entity to attach to
        /// </summary>
        public string entityId { get; set; }

        /// <summary>
        /// When the file is stored in Clarizen, provide the Url you received in a previous request to getUploadUrl
        /// </summary>
        public string uploadUrl { get; set; }

        /// <summary>
        /// Revert image to default icon
        /// </summary>
        public bool reset { get; set; }

        public updateImage(string entityId, string uploadUrl, bool reset)
        {
            this.entityId = entityId;
            this.uploadUrl = uploadUrl;
            this.reset = reset;
        }
    }
}