namespace Ekin.Clarizen
{
    public class OrderBy
    {
        public string FieldName { get; set; }
        /// <summary>
        /// Possible values: Ascending | Descending
        /// </summary>
        public string Order { get; set; }

        public OrderBy() { }

        public OrderBy(string fieldName, string order)
        {
            FieldName = fieldName;
            Order = order;
        }
    }
}