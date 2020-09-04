using Newtonsoft.Json;
using System;

namespace Ekin.Clarizen
{
    public class CalendarException: EntityId
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string ExternalID { get; set; }

        /// <summary>
        /// Indicates whether this day is a working day
        /// </summary>
        public bool? WorkingDay { get; set; }

        /// <summary>
        /// Indicates whether this is an all day exception
        /// </summary>
        public bool? AllDay { get; set; }

        /// <summary>
        /// The date when the exception starts
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// The date when the exception ends
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// The type of the exception
        /// </summary>
        [JsonConverter(typeof(EntityIdConverter))]
        public EntityId ExceptionType { get; set; }

        /// <summary>
        /// The entity this Calendar Exception will be linked to
        /// </summary>
        [JsonConverter(typeof(EntityIdConverter))]
        public EntityId EventCalendar { get; set; }

        #region Fields with problems on the Clarizen API

        // startHour, endHour and repeat were deprecated in the Clarizen API on 22 May 2020 (most probably by error)
        // On 4 Sep 2020 we have brought them back

        /// <summary>
        /// A number between 0 and 24 representing the first working hour of the day. If the day starts at 08:30AM this number will contain 8.5
        /// </summary>
        public double? StartHour { get; set; }

        /// <summary>
        /// A number between 0 and 24 representing the end of the working day. If the day ends at 17:15 this number will contain 17.25
        /// </summary>
        public double? EndHour { get; set; }

        /// <summary>
        /// Recurrence definition
        /// </summary>
        public Repeat Repeat { get; set; }

        #endregion

        public CalendarException() { }
    }
}