namespace Ekin.Clarizen
{
    public class fileInformation
    {
        public string storage { get; set; }
        public string url { get; set; }
        public string fileName { get; set; }
        public string subType { get; set; }
        public string extendedInfo { get; set; }

        public fileInformation()
        {
        }

        public fileInformation(storageType storage, string url, string fileName, string subType, string extendedInfo)
        {
            this.storage = storage.ToEnumString();
            this.url = url;
            this.fileName = fileName;
            this.subType = subType;
            this.extendedInfo = extendedInfo;
        }
    }
}