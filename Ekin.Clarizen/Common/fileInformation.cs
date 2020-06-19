namespace Ekin.Clarizen
{
    public class FileInformation
    {
        public string Storage { get; set; }
        public string Url { get; set; }
        public string FileName { get; set; }
        public string SubType { get; set; }
        public string ExtendedInfo { get; set; }

        public FileInformation() { }

        public FileInformation(StorageType storage, string url, string fileName, string subType, string extendedInfo)
        {
            Storage = storage.ToEnumString();
            Url = url;
            FileName = fileName;
            SubType = subType;
            ExtendedInfo = extendedInfo;
        }
    }
}