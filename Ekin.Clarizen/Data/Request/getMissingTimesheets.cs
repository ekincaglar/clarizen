using System;

namespace Ekin.Clarizen.Data.Request
{
    public class GetMissingTimesheets
    {
        /// <summary>
        /// EntityId of the User. If querying a multi-instance environment use /Organization/orgId/User/userId
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// The start of the date range (inclusive)
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// The end of the date range (exclusive)
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// (Optional) Tolerance in minutes. Default is 5 minutes.
        /// </summary>
        public int? Tolerance { get; set; } = null;

        public GetMissingTimesheets() { }

        public GetMissingTimesheets(string user, DateTime startDate, DateTime endDate)
        {
            User = user;
            StartDate = startDate;
            EndDate = endDate;
        }

        public GetMissingTimesheets(string user, DateTime startDate, DateTime endDate, int tolerance)
        {
            User = user;
            StartDate = startDate;
            EndDate = endDate;
            Tolerance = tolerance;
        }
    }
}