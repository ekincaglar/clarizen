using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen.Data.Result
{
    public class getCalendarInfo
    {
        public string weekStartsOn { get; set; }
        public dayInformation[] weekDayInformation { get; set; }
        public dayInformation defaultWorkingDay { get; set; }
        public int workingDaysPerMonth { get; set; }
    }
}