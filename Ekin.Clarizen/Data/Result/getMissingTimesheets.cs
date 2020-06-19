using System.Collections.Generic;

namespace Ekin.Clarizen.Data.Result
{
    public class GetMissingTimesheets
    {
        public List<DayOfMissingTimesheets> MissingTimesheets { get; set; }

        public GetMissingTimesheets() { }
    }
}
