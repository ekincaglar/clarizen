using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen.Data.Result
{
    public class getCalendarExceptions
    {
        public calendarException[] calendarExceptions { get; set; }
        public exceptionDate[] exceptionDates { get; set; }

        public getCalendarExceptions() { }
    }
}