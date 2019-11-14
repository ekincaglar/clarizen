using System;

namespace Ekin.Clarizen.Data.Request
{
    public class getMissingTimesheets
    {
        /// <summary>
        /// EntityId of the User. If querying a multi-instance environment use /Organization/orgId/User/userId
        /// </summary>
        public string user { get; set; }

        /// <summary>
        /// The start of the date range (inclusive)
        /// </summary>
        public DateTime startDate { get; set; }

        /// <summary>
        /// The end of the date range (exclusive)
        /// </summary>
        public DateTime endDate { get; set; }

        /// <summary>
        /// (Optional) Tolerance in minutes. Default is 5 minutes.
        /// </summary>
        public int? tolerance { get; set; } = null;

        public getMissingTimesheets()
        {
        }

        public getMissingTimesheets(string user, DateTime startDate, DateTime endDate)
        {
            this.user = user;
            this.startDate = startDate;
            this.endDate = endDate;
        }

        public getMissingTimesheets(string user, DateTime startDate, DateTime endDate, int tolerance)
        {
            this.user = user;
            this.startDate = startDate;
            this.endDate = endDate;
            this.tolerance = tolerance;
        }
    }
}