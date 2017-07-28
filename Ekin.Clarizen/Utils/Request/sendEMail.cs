using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen.Utils.Request
{
    public class sendEMail
    {
        public string subject { get; set; }
        public string body { get; set; }
        public recipient[] recipients { get; set; }
        /// <summary>
        /// Entity Id of the related entity
        /// </summary>
        public string relatedEntity { get; set; }
        /// <summary>
        /// Possible values: Public | Private
        /// </summary>
        public string accessType { get; set; }

        //[JsonIgnore]
        //[IgnoreDataMember]
        public enum CZAccessType { Private, Public }

        public sendEMail(recipient[] recipients, string subject, string body, string relatedEntityId, CZAccessType accessType)
        {
            this.recipients = recipients;
            this.subject = subject;
            this.body = body;
            this.relatedEntity = relatedEntityId;
            this.accessType = (accessType == CZAccessType.Private ? "Private" : "Public");
        }

    }
}