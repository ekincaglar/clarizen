using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen
{
    public class calendarException
    {
        public string id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Indicates whether this day is a working day
        /// </summary>
        public bool isWorkingDay { get; set; }

        /// <summary>
        /// Indicates whether this is an all day exception
        /// </summary>
        public bool isAllDay { get; set; }

        /// <summary>
        /// A number between 0 and 24 representing the first working hour of the day. If the day starts at 08:30AM this number will contain 8.5
        /// </summary>
        public double startHour { get; set; }

        /// <summary>
        /// A number between 0 and 24 representing the end of the working day. If the day ends at 17:15 this number will contain 17.25
        /// </summary>
        public double endHour { get; set; }

        /// <summary>
        /// The type of the exception
        /// </summary>
        public string exceptionType { get; set; }

        /// <summary>
        /// The date when the exception starts
        /// </summary>
        public DateTime startDate { get; set; }

        /// <summary>
        /// The date when the exception ends
        /// </summary>
        public DateTime endDate { get; set; }

        /// <summary>
        /// Recurrence definition
        /// </summary>
        public repeat repeat { get; set; }

        public calendarException() { }
    }
}