using Ekin.Clarizen.Interfaces;

namespace Ekin.Clarizen.Data.Queries
{
    public class entityFeedQuery : IQuery
    {
        public string _type { get { return "entityFeedQuery"; } }

        public string entityId { get; set; }
        public string[] fields { get; set; }
        public string[] feedItemOptions { get; set; }
        public paging paging { get; set; }

        public entityFeedQuery(string entityId, string[] fields, string[] feedItemOptions, paging paging)
        {
            this.entityId = entityId;
            this.fields = fields;
            this.feedItemOptions = feedItemOptions;
            this.paging = paging;
        }

        public entityFeedQuery(string entityId, string[] fields)
        {
            this.entityId = entityId;
            this.fields = fields;
        }
    }
}