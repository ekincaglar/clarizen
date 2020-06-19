namespace Ekin.Clarizen.Data.Request
{
    public class Objects_get
    {
        /// <summary>
        /// The list of fields to read
        /// </summary>
        public string[] Fields { get; set; }

        /// <summary>
        /// Represents the unique Id of an entity in Clarizen
        /// Format: /typeName/entityId (e.g. /task/3F2504E0-4F89-42D3-9A0C-0305E82C3301)
        /// </summary>
        public string Id { get; set; }

        public Objects_get (string id, string[] fields)
        {
            Id = id;
            Fields = fields;
        }

    }
}