using Ekin.Clarizen.Interfaces;

namespace Ekin.Clarizen.Data.Queries
{
    public class EntityFeedQuery : IQuery
    {
        public string _type { get { return "entityFeedQuery"; } }

        public string EntityId { get; set; }
        public string[] Fields { get; set; }
        public string[] FeedItemOptions { get; set; }
        public Paging Paging { get; set; }

        public EntityFeedQuery(string entityId, string[] fields, string[] feedItemOptions, Paging paging)
        {
            EntityId = entityId;
            Fields = fields;
            FeedItemOptions = feedItemOptions;
            Paging = paging;
        }

        public EntityFeedQuery(string entityId, string[] fields)
        {
            EntityId = entityId;
            Fields = fields;
        }
    }
}