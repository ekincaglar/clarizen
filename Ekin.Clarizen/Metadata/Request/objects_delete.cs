namespace Ekin.Clarizen.Metadata.Request
{
    public class Objects_delete
    {
        /// <summary>
        /// Represents the unique Id of an entity in Clarizen
        /// Format /typeName/entityId (e.g. /task/3F2504E0-4F89-42D3-9A0C-0305E82C3301)
        /// </summary>
        public string Id { get; set; }

        public Objects_delete(string id)
        {
            Id = id;
        }
    }
}