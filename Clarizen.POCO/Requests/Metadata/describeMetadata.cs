namespace Ekin.Clarizen.Metadata.Request
{
    public class describeMetadata
    {
        /// <summary>
        /// The types of entities to describe. if null, will return information about all entities in the organization
        /// </summary>
        public string[] typeNames { get; set; }

        /// <summary>
        /// (Optional) Flags that define which information to return. If null, result will include only the basic information about each entity type Supported flags: fields, relations
        /// </summary>
        public string[] flags { get; set; }

        public describeMetadata(string[] typeNames, string[] flags)
        {
            this.typeNames = typeNames;
            this.flags = flags;
        }

        public describeMetadata()
        {
            this.typeNames = new string[] { };
            this.flags = new string[] { };
        }
    }
}