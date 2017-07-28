using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen.Data.Request
{
    public class changeState
    {
        /// <summary>
        /// List of objects to perform the operation on
        /// EntityID format: /typeName/entityId (e.g. /task/3F2504E0-4F89-42D3-9A0C-0305E82C3301)
        /// </summary>
        public string[] ids { get; set; }
        /// <summary>
        /// The new state
        /// </summary>
        public string state { get; set; }

        public changeState(string[] ids, string state)
        {
            this.ids = ids;
            this.state = state;
        }
    }
}