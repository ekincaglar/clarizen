namespace Ekin.Clarizen
{
    public enum storageType
    {
        Server, Url, Link
    }

    public static class storageTypeExtensions
    {
        public static string ToEnumString(this storageType me)
        {
            switch (me)
            {
                case storageType.Server: return "Server";
                case storageType.Url: return "Url";
                case storageType.Link: return "Link";
                default: return "ERROR";
            }
        }
    }
}