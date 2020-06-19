namespace Ekin.Clarizen
{
    public class Paging
    {
        /// <summary>
        /// The record number to start retrieve from
        /// </summary>
        public int From { get; set; }
        /// <summary>
        /// Number of records to retrieve
        /// </summary>
        public int Limit { get; set; }
        /// <summary>
        /// When a query results is returned, indicates whether there are more records to fetch for this query
        /// </summary>
        public bool HasMore { get; set; }

        public Paging() { }
    }
}