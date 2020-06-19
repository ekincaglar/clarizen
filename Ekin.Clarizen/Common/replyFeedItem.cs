namespace Ekin.Clarizen
{
    public class ReplyFeedItem
    {
        /// <summary>
        /// The discussion message entity
        /// </summary>
        public dynamic Message { get; set; }

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

        public string BodyMarkup { get; set; }

        public string Summary { get; set; }

        public ReplyFeedItem() { }
    }
}