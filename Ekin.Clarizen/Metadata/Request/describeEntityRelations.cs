namespace Ekin.Clarizen.Metadata.Request
{
    public class DescribeEntityRelations
    {
        /// <summary>
        /// List of types to describe
        /// </summary>
        public string[] TypeNames { get; set; }

        public DescribeEntityRelations(string[] typeNames)
        {
            TypeNames = typeNames;
        }
    }
}