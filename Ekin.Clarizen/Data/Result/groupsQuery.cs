namespace Ekin.Clarizen.Data.Result
{
    public class GroupsQuery
    {
        /// <summary>
        /// Array of entities returned from this query
        /// </summary>
        public dynamic[] Items { get; set; }

        /// <summary>
        /// Paging information returned from this query. If paging.hasMore is true, this object should be passed as is, to the same query API in order to retrieve the next page
        /// </summary>
        public Paging Paging { get; set; }
    }
}