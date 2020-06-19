namespace Ekin.Clarizen.Data.Result
{
    public class GetCalendarExceptions
    {
        public CalendarException[] CalendarExceptions { get; set; }
        public ExceptionDate[] ExceptionDates { get; set; }

        public GetCalendarExceptions() { }
    }
}