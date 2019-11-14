namespace Ekin.Clarizen
{
    public class parameters
    {
        /// <summary>
        /// Bind parameter values in JSON format i.e. {
        /// param1: value1,
        /// param2: value2
        /// }
        /// </summary>
        public object objectFields { get; set; }

        public parameters()
        {
        }

        public parameters(object objectFields)
        {
            this.objectFields = objectFields;
        }
    }
}