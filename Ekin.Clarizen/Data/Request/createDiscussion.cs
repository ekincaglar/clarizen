namespace Ekin.Clarizen.Data.Request
{
    public class CreateDiscussion
    {
        /// <summary>
        /// Discussion message to be created
        /// </summary>
        public object Entity { get; set; }
        /// <summary>
        /// Entity Ids
        /// </summary>
        public string[] RelatedEntities { get; set; }
        /// <summary>
        /// Entity Ids for users or groups to notify
        /// </summary>
        public string[] Notify { get; set; }
        /// <summary>
        /// Entity Ids
        /// </summary>
        public string[] Topics { get; set; }

        public CreateDiscussion(object entity, string[] relatedEntities, string[] notify, string[] topics)
        {
            Entity = entity;
            RelatedEntities = relatedEntities;
            Notify = notify;
            Topics = topics;
        }

        public CreateDiscussion(object entity, string[] relatedEntities, string[] notify)
        {
            Entity = entity;
            RelatedEntities = relatedEntities;
            Notify = notify;
        }

        public CreateDiscussion(object entity, string[] relatedEntities)
        {
            Entity = entity;
            RelatedEntities = relatedEntities;
        }

        public CreateDiscussion(object entity)
        {
            Entity = entity;
        }
    }
}