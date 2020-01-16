using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Ekin.Clarizen
{
    public enum Day
    {
        Sunday, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday
    }

    public static class DayExtensions
    {
        public static string ToEnumString(this Day me)
        {
            switch (me)
            {
                case Day.Sunday: return "Sunday";
                case Day.Monday: return "Monday";
                case Day.Tuesday: return "Tuesday";
                case Day.Wednesday: return "Wednesday";
                case Day.Thursday: return "Thursday";
                case Day.Friday: return "Friday";
                case Day.Saturday: return "Saturday";
                default: return "ERROR";
            }
        }
    }
}