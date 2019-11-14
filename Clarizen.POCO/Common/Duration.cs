using System;

namespace Ekin.Clarizen
{
    public class Duration
    {
        /// <summary>
        /// E.g. Days
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// E.g. 5.0
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// E.g. Default
        /// </summary>
        public string DurationType { get; set; }

        public Duration()
        {
        }

        public Duration(string unit, double value, string durationType, int roundToDecimalPlaces = 2)
        {
            Unit = unit;
            Value = Math.Round(value, roundToDecimalPlaces);
            DurationType = durationType;
        }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }

            var item = obj as Duration;

            if (item == null)
            {
                return false;
            }

            bool unitEquals = true;
            if (!string.IsNullOrWhiteSpace(Unit) && !string.IsNullOrWhiteSpace(item.Unit))
            {
                unitEquals = Unit.Equals(item.Unit, StringComparison.InvariantCultureIgnoreCase);
            }

            bool durationTypeEquals = true;
            if (!string.IsNullOrWhiteSpace(DurationType) && !string.IsNullOrWhiteSpace(item.DurationType))
            {
                durationTypeEquals = DurationType.Equals(item.DurationType, StringComparison.InvariantCultureIgnoreCase);
            }

            return unitEquals && durationTypeEquals && Value == item.Value;
        }

        public override int GetHashCode()
        {
            return string.Format("{0}{1}{2}",
                string.IsNullOrWhiteSpace(Unit) ? "" : Unit.ToLowerInvariant(),
                Value,
                string.IsNullOrWhiteSpace(DurationType) ? "" : DurationType.ToLowerInvariant()).GetHashCode();
        }
    }
}