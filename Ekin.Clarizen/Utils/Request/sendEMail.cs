namespace Ekin.Clarizen.Utils.Request
{
    public class SendEmail
    {
        public string Subject { get; set; }

        public string Body { get; set; }

        public Recipient[] Recipients { get; set; }

        /// <summary>
        /// Entity Id of the related entity
        /// </summary>
        public string RelatedEntity { get; set; }

        /// <summary>
        /// Possible values: Public | Private
        /// </summary>
        public string AccessType { get; set; }

        //[JsonIgnore]
        //[IgnoreDataMember]
        public enum CZAccessType { Private, Public }

        public SendEmail(Recipient[] recipients, string subject, string body, string relatedEntityId, CZAccessType accessType)
        {
            Recipients = recipients;
            Subject = subject;
            Body = body;
            RelatedEntity = relatedEntityId;
            AccessType = (accessType == CZAccessType.Private ? "Private" : "Public");
        }
    }
}