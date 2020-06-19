using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Ekin.Log;
using Newtonsoft.Json;

namespace Ekin.Clarizen
{
    public class FileUploadHelper
    {
        #region Private POCOs

        private class Document : EntityId
        {
            public string Name { get; set; }

            [JsonConverter(typeof(EntityIdConverter))]
            public EntityId DocumentType { get; set; }

            [JsonConverter(typeof(EntityIdConverter))]
            public EntityId FileType { get; set; }
        }

        private class AttachmentLink : EntityId
        {
            [JsonConverter(typeof(EntityIdConverter))]
            public EntityId Entity { get; set; }

            [JsonConverter(typeof(EntityIdConverter))]
            public EntityId Document { get; set; }
        }

        #endregion Private POCOs

        public API ClarizenAPI { get; private set; }
        public LogFactory Logs { get; set; }

        public FileUploadHelper(API ClarizenAPI)
        {
            Logs = new LogFactory();
            this.ClarizenAPI = ClarizenAPI;
        }

        public void UseSession(API ClarizenAPI)
        {
            this.ClarizenAPI = ClarizenAPI;
        }

        /// <summary>
        /// Uploads a file and links it to an entity
        /// </summary>
        /// <param name="InputData">Either a string or a byte[] to upload</param>
        /// <param name="DocumentName">Name of the document entity in Clarizen</param>
        /// <param name="FileType">File Type in Clarizen, e.g. /FileType/PDF</param>
        /// <param name="DocumentType">Document Type in Clarizen, e.g. /DocumentType/YourCustomType (Optional)</param>
        /// <param name="LinkedEntity">An entity in Clarizen to link the uploaded file to (Optional)</param>
        /// <returns></returns>
        public async Task<bool> Upload(object InputData, string DocumentName, string FileName, EntityId FileType, EntityId DocumentType = null, EntityId LinkedEntity = null)
        {
            Logs = new Ekin.Log.LogFactory();

            #region Validate input

            if (InputData == null)
            {
                Logs.AddError("Ekin.Clarizen.FileUploadHelper", "Upload", "InputData cannot be null");
                return false;
            }

            if (!(InputData is string || InputData is byte[]))
            {
                Logs.AddError("Ekin.Clarizen.FileUploadHelper", "Upload", "InputData should be a string or a byte array");
                return false;
            }

            #endregion Validate input

            #region Create a Document entity

            Document document = new Document
            {
                Id = "/Document",
                Name = DocumentName,
                FileType = FileType,
                DocumentType = DocumentType
            };

            Ekin.Clarizen.Data.Objects_put clarizenDocument = await ClarizenAPI.CreateObject(document.Id, document);
            if (!clarizenDocument.IsCalledSuccessfully || clarizenDocument.Data == null || string.IsNullOrWhiteSpace(clarizenDocument.Data.Id))
            {
                Logs.AddError("Ekin.Clarizen.FileUploadHelper", "Upload", "Blank document couldn't be created in Clarizen. Error: " + clarizenDocument.Error);
                return false;
            }
            else
            {
                document.Id = clarizenDocument.Data.Id;
            }

            #endregion Create a Document entity

            #region Get Upload URL from Clarizen

            Ekin.Clarizen.Files.GetUploadUrl uploadUrlCall = await ClarizenAPI.GetUploadUrl();
            if (!uploadUrlCall.IsCalledSuccessfully || uploadUrlCall.Data == null || string.IsNullOrWhiteSpace(uploadUrlCall.Data.UploadUrl))
            {
                Logs.AddError("Ekin.Clarizen.FileUploadHelper", "Upload", "Document upload url couldn't be retrieved from Clarizen. Error: " + uploadUrlCall.Error);
                return false;
            }

            string uploadUrl = uploadUrlCall.Data.UploadUrl;

            #endregion Get Upload URL from Clarizen

            #region Send the file to the Upload URL

            using (var client = new HttpClient())
            {
                using (var formData = new MultipartFormDataContent())
                {
                    if (InputData is string)
                    {
                        formData.Add(new StringContent((string)InputData), "file", FileName);
                    }
                    else if (InputData is byte[])
                    {
                        formData.Add(new ByteArrayContent((byte[])InputData), "file", FileName);
                    }
                    HttpResponseMessage response = await client.PostAsync(uploadUrl, formData).ConfigureAwait(false);
                    if (!response.IsSuccessStatusCode)
                    {
                        using (HttpContent content = response.Content)
                        {
                            string data = await content.ReadAsStringAsync().ConfigureAwait(false);
                            Logs.AddError("Ekin.Clarizen.FileUploadHelper", "Upload", "Document couldn't be uploaded. Error: " + data);
                        }
                        return false;
                    }
                    System.IO.Stream resultStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                }
            }

            #endregion Send the file to the Upload URL

            #region Complete the Upload

            var subType = "";
            var extendedInfo = "";
            var fileInformation = new FileInformation(StorageType.Server, document.Id, FileName, subType, extendedInfo);
            var uploadRequest = new Ekin.Clarizen.Files.Request.Upload(document.Id, fileInformation, uploadUrl);
            CallSettings callSettings = CallSettings.GetFromAPI(ClarizenAPI);
            callSettings.IsBulk = false;
            var uploadCall = new Ekin.Clarizen.Files.Upload(uploadRequest, callSettings);
            bool uploadResult = await uploadCall.Execute();
            if (!uploadResult)
            {
                Logs.AddError("Ekin.Clarizen.FileUploadHelper", "Upload", "Document couldn't be uploaded to Clarizen. Error: " + uploadCall.Error);
                return false;
            }

            #endregion Complete the Upload

            #region If a LinkedEntity object is provided link the file to it

            if (LinkedEntity != null)
            {
                AttachmentLink attachmentLink = new AttachmentLink
                {
                    Id = "/AttachmentLink",
                    Entity = LinkedEntity,
                    Document = new EntityId(document.Id)
                };

                Ekin.Clarizen.Data.Objects_put clarizenDocumentLink = await ClarizenAPI.CreateObject(attachmentLink.Id, attachmentLink);
                if (!clarizenDocumentLink.IsCalledSuccessfully || clarizenDocumentLink.Data == null || clarizenDocumentLink.Data.Id == null)
                {
                    Logs.AddError("Ekin.Clarizen.FileUploadHelper", "Upload", "Document successfully created in Clarizen but it could not be linked to the System Admin user. Error: " + clarizenDocumentLink.Error);
                    return false;
                }
            }

            #endregion If a LinkedEntity object is provided link the file to it

            return true;
        }
    }
}