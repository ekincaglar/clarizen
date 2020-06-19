namespace Ekin.Clarizen.Data.Result
{
    public class GetCalendarInfo
    {
        public string WeekStartsOn { get; set; }
        public DayInformation[] WeekDayInformation { get; set; }
        public DayInformation DefaultWorkingDay { get; set; }
        public int WorkingDaysPerMonth { get; set; }

        public GetCalendarInfo() { }
    }
}