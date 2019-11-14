namespace Ekin.Clarizen.Metadata.Request
{
    public class describeEntityRelations
    {
        /// <summary>
        /// List of types to describe
        /// </summary>
        public string[] typeNames { get; set; }

        public describeEntityRelations(string[] typeNames)
        {
            this.typeNames = typeNames;
        }
    }
}