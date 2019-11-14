using System;

namespace Ekin.Clarizen
{
    public class repeat
    {
        public every every { get; set; }
        public int occurrences { get; set; }
        public DateTime endBy { get; set; }

        public repeat()
        {
        }
    }
}