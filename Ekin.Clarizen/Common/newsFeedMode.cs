namespace Ekin.Clarizen
{
    public enum NewsFeedMode
    {
        Following, All
    }

    public static class NewsFeedModeExtensions
    {
        public static string ToEnumString(this NewsFeedMode me)
        {
            switch (me)
            {
                case NewsFeedMode.All: return "All";
                case NewsFeedMode.Following: return "Following";
                default: return "ERROR";
            }
        }
    }
}