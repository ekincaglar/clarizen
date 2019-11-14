namespace Ekin.Clarizen.Data.Request
{
    public class createDiscussion
    {
        /// <summary>
        /// Discussion message to be created
        /// </summary>
        public object entity { get; set; }

        /// <summary>
        /// Entity Ids
        /// </summary>
        public string[] relatedEntities { get; set; }

        /// <summary>
        /// Entity Ids for users or groups to notify
        /// </summary>
        public string[] notify { get; set; }

        /// <summary>
        /// Entity Ids
        /// </summary>
        public string[] topics { get; set; }

        public createDiscussion(object entity, string[] relatedEntities, string[] notify, string[] topics)
        {
            this.entity = entity;
            this.relatedEntities = relatedEntities;
            this.notify = notify;
            this.topics = topics;
        }

        public createDiscussion(object entity, string[] relatedEntities, string[] notify)
        {
            this.entity = entity;
            this.relatedEntities = relatedEntities;
            this.notify = notify;
        }

        public createDiscussion(object entity, string[] relatedEntities)
        {
            this.entity = entity;
            this.relatedEntities = relatedEntities;
        }

        public createDiscussion(object entity)
        {
            this.entity = entity;
        }
    }
}