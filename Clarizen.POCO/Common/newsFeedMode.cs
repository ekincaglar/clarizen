namespace Ekin.Clarizen
{
    public enum newsFeedMode
    {
        Following, All
    }

    public static class newsFeedModeExtensions
    {
        public static string ToEnumString(this newsFeedMode me)
        {
            switch (me)
            {
                case newsFeedMode.All: return "All";
                case newsFeedMode.Following: return "Following";
                default: return "ERROR";
            }
        }
    }
}