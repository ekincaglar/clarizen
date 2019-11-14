using Ekin.Clarizen.Interfaces;

namespace Ekin.Clarizen.Data.Queries
{
    public class repliesQuery : IQuery
    {
        public string _type { get { return "repliesQuery"; } }

        public string postId { get; set; }
        public string[] fields { get; set; }
        public string[] feedItemOptions { get; set; }
        public paging paging { get; set; }

        public repliesQuery(string postId, string[] fields, string[] feedItemOptions, paging paging)
        {
            this.postId = postId;
            this.fields = fields;
            this.feedItemOptions = feedItemOptions;
            this.paging = paging;
        }

        public repliesQuery(string postId, string[] fields)
        {
            this.postId = postId;
            this.fields = fields;
        }
    }
}