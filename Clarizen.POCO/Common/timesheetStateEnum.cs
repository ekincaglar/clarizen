namespace Ekin.Clarizen
{
    public enum timesheetStateEnum
    {
        UnSubmitted, PendingApproval, Approved, All
    }

    public static class timesheetStateEnumExtensions
    {
        public static string ToEnumString(this timesheetStateEnum me)
        {
            switch (me)
            {
                case timesheetStateEnum.UnSubmitted: return "UnSubmitted";
                case timesheetStateEnum.PendingApproval: return "PendingApproval";
                case timesheetStateEnum.Approved: return "Approved";
                case timesheetStateEnum.All: return "All";
                default: return "ERROR";
            }
        }
    }
}