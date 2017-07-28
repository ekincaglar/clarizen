using Ekin.Clarizen.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen
{
    public class relation
    {
        /// <summary>
        /// Relation name
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// A list of field names to retrieve
        /// </summary>
        public string[] fields { get; set; }
        /// <summary>
        /// The query criteria
        /// </summary>
        public ICondition where { get; set; }
        /// <summary>
        /// Optionaly order the result
        /// </summary>
        public orderBy[] orders { get; set; }
        /// <summary>
        /// The query result of link objects
        /// </summary>
        public bool fromLink { get; set; }

        public relation(string name, string[] fields, ICondition where, orderBy[] orders, bool fromLink)
        {
            this.name = name;
            this.fields = fields;
            this.where = where;
            this.orders = orders;
            this.fromLink = fromLink;
        }

    }
}