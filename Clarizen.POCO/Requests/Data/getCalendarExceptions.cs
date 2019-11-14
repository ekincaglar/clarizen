using System;

namespace Ekin.Clarizen.Data.Request
{
    public class getCalendarExceptions
    {
        /// <summary>
        /// (Optional) If EntityId is specified, the result will contain information about the claendar of the specified user or project, otherwise it will be of the organizational calendar
        /// </summary>
        public string entityId { get; set; }

        /// <summary>
        /// The start of the date range
        /// </summary>
        public DateTime fromDate { get; set; }

        /// <summary>
        /// The end of the date range
        /// </summary>
        public DateTime toDate { get; set; }

        public getCalendarExceptions()
        {
        }

        public getCalendarExceptions(string entityId, DateTime fromDate, DateTime toDate)
        {
            this.entityId = entityId;
            this.fromDate = fromDate;
            this.toDate = toDate;
        }

        public getCalendarExceptions(DateTime fromDate, DateTime toDate)
        {
            this.entityId = null;
            this.fromDate = fromDate;
            this.toDate = toDate;
        }
    }
}