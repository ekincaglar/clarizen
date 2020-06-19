namespace Ekin.Clarizen
{
    public enum RequestMethod { Get, Post, Put, Delete }

    public static class RequestMethodExtensions
    {
        public static string ToEnumString(this RequestMethod me)
        {
            switch (me)
            {
                case RequestMethod.Get: return "GET";
                case RequestMethod.Post: return "POST";
                case RequestMethod.Put: return "PUT";
                case RequestMethod.Delete: return "DELETE";
                default: return "ERROR";
            }
        }
    }
}