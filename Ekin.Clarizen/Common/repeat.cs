using System;

namespace Ekin.Clarizen
{
    public class Repeat
    {
        public Every Every { get; set; }
        public int Occurrences { get; set; }
        public DateTime EndBy { get; set; }

        public Repeat() { }

    }
}