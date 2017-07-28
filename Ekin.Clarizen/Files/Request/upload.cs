using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen.Files.Request
{
    public class upload
    {
        /// <summary>
        /// Id of a document to attach to
        /// </summary>
        public string documentId { get; set; }
        /// <summary>
        /// Additional information about the file
        /// </summary>
        public fileInformation fileInformation { get; set; }
        /// <summary>
        /// When the file is stored in Clarizen, provide the Url you received in a previous request to getUploadUrl
        /// </summary>
        public string uploadUrl { get; set; }

        public upload(string documentId, fileInformation fileInformation, string uploadUrl)
        {
            this.documentId = documentId;
            this.fileInformation = fileInformation;
            this.uploadUrl = uploadUrl;
        }
    }
}