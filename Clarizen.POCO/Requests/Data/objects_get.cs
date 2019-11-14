namespace Ekin.Clarizen.Data.Request
{
    public class objects_get
    {
        /// <summary>
        /// The list of fields to read
        /// </summary>
        public string[] fields { get; set; }

        /// <summary>
        /// Represents the unique Id of an entity in Clarizen
        /// Format: /typeName/entityId (e.g. /task/3F2504E0-4F89-42D3-9A0C-0305E82C3301)
        /// </summary>
        public string id { get; set; }

        public objects_get(string id, string[] fields)
        {
            this.id = id;
            this.fields = fields;
        }
    }
}