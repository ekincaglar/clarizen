using System;
using System.Collections.Generic;
using System.Text;

namespace Ekin.Clarizen.Data.Queries.Filters
{
    public class UserDateFilter
    {
        public string User { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
