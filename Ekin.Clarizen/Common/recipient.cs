namespace Ekin.Clarizen
{
    public class Recipient
    {
        /// <summary>
        /// Possible values: To | CC
        /// </summary>
        public string RecipientType { get; set; }

        /// <summary>
        /// EntityId, e.g. /User/theuserguidhere
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Email address
        /// </summary>
        public string Email { get; set; }

        public enum CZRecipientType { To, CC }

        public Recipient() { }

        public Recipient(CZRecipientType recipientType, string eMail, string clarizenUserId)
        {
            this.RecipientType = (recipientType == CZRecipientType.CC ? "CC" : "To");
            this.Email = eMail;
            this.User = clarizenUserId;
        }
    }
}