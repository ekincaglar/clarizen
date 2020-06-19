namespace Ekin.Clarizen.Metadata.Result
{
    public class Objects_put
    {
        /// <summary>
        /// Represents the unique Id of an entity in Clarizen
        /// Format /TypeName/EntityId (e.g. /Task/3F2504E0-4F89-42D3-9A0C-0305E82C3301)
        /// </summary>
        public string Id { get; set; }

        public Objects_put(string id)
        {
            Id = id;
        }
    }
}