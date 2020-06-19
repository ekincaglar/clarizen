using System;

namespace Ekin.Clarizen.Data.Request
{
    public class GetCalendarExceptions
    {
        /// <summary>
        /// (Optional) If EntityId is specified, the result will contain information about the claendar of the specified user or project, otherwise it will be of the organizational calendar
        /// </summary>
        public string EntityId { get; set; }

        /// <summary>
        /// The start of the date range
        /// </summary>
        public DateTime FromDate { get; set; }

        /// <summary>
        /// The end of the date range
        /// </summary>
        public DateTime ToDate { get; set; }

        public GetCalendarExceptions() { }

        public GetCalendarExceptions(string entityId, DateTime fromDate, DateTime toDate)
        {
            EntityId = entityId;
            FromDate = fromDate;
            ToDate = toDate;
        }

        public GetCalendarExceptions(DateTime fromDate, DateTime toDate)
        {
            EntityId = null;
            FromDate = fromDate;
            ToDate = toDate;
        }
    }
}