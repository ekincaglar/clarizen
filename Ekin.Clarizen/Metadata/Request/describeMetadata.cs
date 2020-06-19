namespace Ekin.Clarizen.Metadata.Request
{
    public class DescribeMetadata
    {
        /// <summary>
        /// The types of entities to describe. if null, will return information about all entities in the organization
        /// </summary>
        public string[] TypeNames { get; set; }

        /// <summary>
        /// (Optional) Flags that define which information to return. If null, result will include only the basic information about each entity type Supported flags: fields, relations
        /// </summary>
        public string[] Flags { get; set; }

        public DescribeMetadata(string[] typeNames, string[] flags)
        {
            TypeNames = typeNames;
            Flags = flags;
        }

        public DescribeMetadata()
        {
            TypeNames = new string[] { };
            Flags = new string[] { };
        }
    }
}