namespace Ekin.Clarizen
{
    public class DayInformation
    {
        /// <summary>
        /// Indicates whether this day is a working day
        /// </summary>
        public bool IsWorkingDay { get; set; }
        /// <summary>
        /// Total number of working hour in this day. Only relevant if is true.
        /// </summary>
        public double TotalWorkingHours { get; set; }
        /// <summary>
        /// A number between 0 and 24 representing the first working hour of the day. If the day starts at 08:30AM this number will contain 8.5
        /// </summary>
        public double StartHour { get; set; }
        /// <summary>
        /// A number between 0 and 24 representing the end of the working day. If the day ends at 17:15 this number will contain 17.25
        /// </summary>
        public double EndHour { get; set; }

        public DayInformation() { }

        public DayInformation(bool isWorkingDay, double totalWorkingHours, double startHour, double endHour)
        {
            this.IsWorkingDay = isWorkingDay;
            this.TotalWorkingHours = totalWorkingHours;
            this.StartHour = startHour;
            this.EndHour = endHour;
        }
    }
}