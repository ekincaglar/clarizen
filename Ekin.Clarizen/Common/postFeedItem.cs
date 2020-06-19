namespace Ekin.Clarizen
{
    public class PostFeedItem
    {
        /// <summary>
        /// The discussion message entity
        /// </summary>
        public dynamic Message { get; set; }

        public string BodyMarkup { get; set; }

        public string Summary { get; set; }

        /// <summary>
        /// A flag indicating whether this message is pinned\highlighted
        /// </summary>
        public bool Pinned { get; set; }

        /// <summary>
        /// Boolean flag which indicates whether the current user likes this message
        /// </summary>
        public bool LikedByMe { get; set; }

        /// <summary>
        /// Related items mentioned in this message
        /// </summary>
        public dynamic[] RelatedEntities { get; set; }

        /// <summary>
        /// Users or groups notified in this message
        /// </summary>
        public dynamic[] Notify { get; set; }

        /// <summary>
        /// Topics in this message
        /// </summary>
        public dynamic[] Topics { get; set; }

        /// <summary>
        /// Last two replies in this post
        /// </summary>
        public ReplyFeedItem[] LatestReplies { get; set; }

        public PostFeedItem() { }

        //public postFeedItem(dynamic message, string bodyMarkup, string summary, bool pinned, bool likedByMe, dynamic[] relatedEntities, dynamic[] notify, dynamic[] topics, replyFeedItem[] latestReplies)
        //{
        //    this.message = message;
        //    this.bodyMarkup = bodyMarkup;
        //    this.summary = summary;
        //    this.pinned = pinned;
        //    this.likedByMe = likedByMe;
        //    this.relatedEntities = relatedEntities;
        //    this.notify = notify;
        //    this.topics = topics;
        //    this.latestReplies = latestReplies;
        //}
    }
}