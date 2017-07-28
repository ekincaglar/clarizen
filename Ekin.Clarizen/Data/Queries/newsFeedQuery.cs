using Ekin.Clarizen.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen.Data.Queries
{
    public class newsFeedQuery : IQuery
    {
        public string _type { get { return "newsFeedQuery"; } }

        public string mode { get; set; }
        public string[] fields { get; set; }
        public string[] feedItemOptions { get; set; }
        public paging paging { get; set; }

        public newsFeedQuery(newsFeedMode mode, string[] fields, string[] feedItemOptions, paging paging)
        {
            this.mode = mode.ToEnumString();
            this.fields = fields;
            this.feedItemOptions = feedItemOptions;
            this.paging = paging;
        }

        public newsFeedQuery(newsFeedMode mode, string[] fields)
        {
            this.mode = mode.ToEnumString();
            this.fields = fields;
        }

    }
}