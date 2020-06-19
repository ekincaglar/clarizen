namespace Ekin.Clarizen
{
    public enum StorageType
    {
        Server, Url, Link
    }

    public static class StorageTypeExtensions
    {
        public static string ToEnumString(this StorageType me)
        {
            switch (me)
            {
                case StorageType.Server: return "Server";
                case StorageType.Url: return "Url";
                case StorageType.Link: return "Link";
                default: return "ERROR";
            }
        }
    }
}