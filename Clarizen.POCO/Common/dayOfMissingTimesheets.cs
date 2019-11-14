using System;

namespace Ekin.Clarizen
{
    public class dayOfMissingTimesheets
    {
        public DateTime? date { get; set; }
        public double? workingHours { get; set; }
        public double? reportedHours { get; set; }
        public double? missingHours { get; set; }

        public dayOfMissingTimesheets()
        {
        }
    }
}