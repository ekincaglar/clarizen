using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen
{
    public class postFeedItem
    {
        /// <summary>
        /// The discussion message entity
        /// </summary>
        public dynamic message { get; set; }
        public string bodyMarkup { get; set; }
        public string summary { get; set; }
        /// <summary>
        /// A flag indicating whether this message is pinned\highlighted
        /// </summary>
        public bool pinned { get; set; }
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
        /// <summary>
        /// Last two replies in this post
        /// </summary>
        public replyFeedItem[] latestReplies { get; set; }

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