namespace Ekin.Clarizen
{
    public class dayInformation
    {
        /// <summary>
        /// Indicates whether this day is a working day
        /// </summary>
        public bool isWorkingDay { get; set; }

        /// <summary>
        /// Total number of working hour in this day. Only relevant if is true.
        /// </summary>
        public double totalWorkingHours { get; set; }

        /// <summary>
        /// A number between 0 and 24 representing the first working hour of the day. If the day starts at 08:30AM this number will contain 8.5
        /// </summary>
        public double startHour { get; set; }

        /// <summary>
        /// A number between 0 and 24 representing the end of the working day. If the day ends at 17:15 this number will contain 17.25
        /// </summary>
        public double endHour { get; set; }

        public dayInformation()
        {
        }

        public dayInformation(bool isWorkingDay, double totalWorkingHours, double startHour, double endHour)
        {
            this.isWorkingDay = isWorkingDay;
            this.totalWorkingHours = totalWorkingHours;
            this.startHour = startHour;
            this.endHour = endHour;
        }
    }
}