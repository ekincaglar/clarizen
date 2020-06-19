using Ekin.Clarizen.Interfaces;

namespace Ekin.Clarizen.Data.Queries
{
    public class RepliesQuery : IQuery
    {
        public string _type { get { return "repliesQuery"; } }

        public string PostId { get; set; }
        public string[] Fields { get; set; }
        public string[] FeedItemOptions { get; set; }
        public Paging Paging { get; set; }

        public RepliesQuery(string postId, string[] fields, string[] feedItemOptions, Paging paging)
        {
            PostId = postId;
            Fields = fields;
            FeedItemOptions = feedItemOptions;
            Paging = paging;
        }

        public RepliesQuery(string postId, string[] fields)
        {
            PostId = postId;
            Fields = fields;
        }
    }
}