using System;

namespace Ekin.Clarizen
{
    public class DayOfMissingTimesheets
    {
        public DateTime? Date { get; set; }
        public double? WorkingHours { get; set; }
        public double? ReportedHours { get; set; }
        public double? MissingHours { get; set; }

        public DayOfMissingTimesheets() { }
    }
}