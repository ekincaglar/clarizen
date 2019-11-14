namespace Ekin.Clarizen.Data.Result
{
    public class getCalendarInfo
    {
        public string weekStartsOn { get; set; }
        public dayInformation[] weekDayInformation { get; set; }
        public dayInformation defaultWorkingDay { get; set; }
        public int workingDaysPerMonth { get; set; }

        public getCalendarInfo()
        {
        }
    }
}