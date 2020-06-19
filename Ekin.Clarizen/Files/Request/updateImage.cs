namespace Ekin.Clarizen.Files.Request
{
    public class UpdateImage
    {
        /// <summary>
        /// Id of an entity to attach to
        /// </summary>
        public string EntityId { get; set; }

        /// <summary>
        /// When the file is stored in Clarizen, provide the Url you received in a previous request to getUploadUrl
        /// </summary>
        public string UploadUrl { get; set; }

        /// <summary>
        /// Revert image to default icon
        /// </summary>
        public bool Reset { get; set; }

        public UpdateImage(string entityId, string uploadUrl, bool reset)
        {
            EntityId = entityId;
            UploadUrl = uploadUrl;
            Reset = reset;
        }
    }
}