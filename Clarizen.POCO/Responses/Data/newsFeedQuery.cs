namespace Ekin.Clarizen.Data.Result
{
    public class newsFeedQuery
    {
        /// <summary>
        /// Array of items returned from this query
        /// </summary>
        public postFeedItem[] items { get; set; }

        /// <summary>
        /// Paging information returned from this query. If paging.hasMore is true, this object should be passed as is, to the same query API in order to retrieve the next page
        /// </summary>
        public paging paging { get; set; }
    }
}