using Ekin.Clarizen.Interfaces;

namespace Ekin.Clarizen.Data.Queries
{
    public class NewsFeedQuery : IQuery
    {
        public string _type { get { return "newsFeedQuery"; } }

        public string Mode { get; set; }
        public string[] Fields { get; set; }
        public string[] FeedItemOptions { get; set; }
        public Paging Paging { get; set; }

        public NewsFeedQuery(NewsFeedMode mode, string[] fields, string[] feedItemOptions, Paging paging)
        {
            Mode = mode.ToEnumString();
            Fields = fields;
            FeedItemOptions = feedItemOptions;
            Paging = paging;
        }

        public NewsFeedQuery(NewsFeedMode mode, string[] fields)
        {
            Mode = mode.ToEnumString();
            Fields = fields;
        }
    }
}