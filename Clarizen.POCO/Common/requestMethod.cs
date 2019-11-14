namespace Ekin.Clarizen
{
    public enum requestMethod { Get, Post, Put, Delete }

    public static class requestMethodExtensions
    {
        public static string ToEnumString(this requestMethod me)
        {
            switch (me)
            {
                case requestMethod.Get: return "GET";
                case requestMethod.Post: return "POST";
                case requestMethod.Put: return "PUT";
                case requestMethod.Delete: return "DELETE";
                default: return "ERROR";
            }
        }
    }
}