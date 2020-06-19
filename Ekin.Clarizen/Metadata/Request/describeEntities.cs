namespace Ekin.Clarizen.Metadata.Request
{
    public class DescribeEntities
    {
        /// <summary>
        /// The types of entities to describe
        /// </summary>
        public string[] TypeNames { get; set; }

        public DescribeEntities(string[] typeNames)
        {
            TypeNames = typeNames;
        }
    }
}