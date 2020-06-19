namespace Ekin.Clarizen
{
    public enum TimesheetStateEnum
    {
        UnSubmitted, PendingApproval, Approved, All
    }

    public static class TimesheetStateEnumExtensions
    {
        public static string ToEnumString(this TimesheetStateEnum me)
        {
            switch (me)
            {
                case TimesheetStateEnum.UnSubmitted: return "UnSubmitted";
                case TimesheetStateEnum.PendingApproval: return "PendingApproval";
                case TimesheetStateEnum.Approved: return "Approved";
                case TimesheetStateEnum.All: return "All";
                default: return "ERROR";
            }
        }
    }
}