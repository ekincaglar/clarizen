namespace Ekin.Clarizen
{
    public class Parameters
    {
        /// <summary>
        /// Bind parameter values in JSON format i.e. {
        /// param1: value1, 
        /// param2: value2
        /// }
        /// </summary>
        public object ObjectFields { get; set; }

        public Parameters() { }

        public Parameters(object objectFields)
        {
            ObjectFields = objectFields;
        }
    }
}