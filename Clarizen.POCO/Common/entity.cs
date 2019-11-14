namespace Ekin.Clarizen
{
    public class entity
    {
        /// <summary>
        /// Represents the unique Id of an entity in Clarizen
        /// Format /typeName/entityId (e.g. /task/3F2504E0-4F89-42D3-9A0C-0305E82C3301)
        /// </summary>
        public string id { get; set; }

        public dynamic objectFields { get; set; }

        public entity()
        {
        }
    }
}