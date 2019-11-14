namespace Ekin.Clarizen
{
    public class replyFeedItem
    {
        /// <summary>
        /// The discussion message entity
        /// </summary>
        public dynamic message { get; set; }

        /// <summary>
        /// Boolean flag which indicates whether the current user likes this message
        /// </summary>
        public bool likedByMe { get; set; }

        /// <summary>
        /// Related items mentioned in this message
        /// </summary>
        public dynamic[] relatedEntities { get; set; }

        /// <summary>
        /// Users or groups notified in this message
        /// </summary>
        public dynamic[] notify { get; set; }

        /// <summary>
        /// Topics in this message
        /// </summary>
        public dynamic[] topics { get; set; }

        public string bodyMarkup { get; set; }
        public string summary { get; set; }

        public replyFeedItem()
        {
        }

        //public replyFeedItem(dynamic message, bool likedByMe, dynamic[] relatedEntities, dynamic[] notify, dynamic[] topics, string bodyMarkup, string summary)
        //{
        //    this.message = message;
        //    this.likedByMe = likedByMe;
        //    this.relatedEntities = relatedEntities;
        //    this.notify = notify;
        //    this.topics = topics;
        //    this.bodyMarkup = bodyMarkup;
        //    this.summary = summary;
        //}
    }
}