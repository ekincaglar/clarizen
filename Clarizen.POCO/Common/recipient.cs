namespace Ekin.Clarizen
{
    public class recipient
    {
        /// <summary>
        /// Possible values: To | CC
        /// </summary>
        public string recipientType { get; set; }

        /// <summary>
        /// EntityId, e.g. /User/theuserguidhere
        /// </summary>
        public string user { get; set; }

        /// <summary>
        /// Email address
        /// </summary>
        public string eMail { get; set; }

        public enum CZRecipientType { To, CC }

        public recipient()
        {
        }

        public recipient(CZRecipientType recipientType, string eMail, string clarizenUserId)
        {
            this.recipientType = (recipientType == CZRecipientType.CC ? "CC" : "To");
            this.eMail = eMail;
            this.user = clarizenUserId;
        }
    }
}