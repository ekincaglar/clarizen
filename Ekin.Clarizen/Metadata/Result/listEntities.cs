using System;

namespace Ekin.Clarizen.Metadata.Result
{
    public class ListEntities
    {
        public string[] TypeNames { get; set; }

        public void SortTypeNames()
        {
            Array.Sort(TypeNames);
        }
    }
}