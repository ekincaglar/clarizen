namespace Ekin.Clarizen.Data.Result
{
    public class timesheetQuery
    {
        /// <summary>
        /// Array of entities returned from this query
        /// </summary>
        public dynamic[] entities { get; set; }

        /// <summary>
        /// Paging information returned from this query. If paging.hasMore is true, this object should be passed as is, to the same query API in order to retrieve the next page
        /// </summary>
        public paging paging { get; set; }
    }
}