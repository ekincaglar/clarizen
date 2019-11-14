using System;

namespace Ekin.Clarizen.Metadata.Result
{
    public class listEntities
    {
        public string[] typeNames { get; set; }

        public void SortTypeNames()
        {
            Array.Sort(typeNames);
        }
    }
}