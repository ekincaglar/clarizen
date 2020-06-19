namespace Ekin.Clarizen
{
    /// <summary>
    /// Define an aggregation to perform in an aggregate query 
    /// </summary>
    public class FieldAggregation
    {
        /// <summary>
        /// Type of aggregate function to perform (e.g. Count, Sum, etc.)
        /// </summary>
        public string Function { get; set; }

        /// <summary>
        /// Name of a field to perform this function on
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// (Optional) A name that will represent the result of this function. If this isn't provided, a default name will be generated (e.g. Max_PercentCompleted when performing a Max function on the PercentCompleted field)
        /// </summary>
        public string Alias { get; set; }

        public FieldAggregation() { }

        public FieldAggregation(string function, string fieldName, string alias)
        {
            Function = function;
            FieldName = fieldName;
            Alias = alias;
        }
    }
}