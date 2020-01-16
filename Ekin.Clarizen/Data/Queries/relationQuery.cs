using Ekin.Clarizen.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen.Data.Queries
{
    public class relationQuery
    {
        /// <summary>
        /// Represents the unique Id of an entity in Clarizen
        /// Format: /typeName/entityId (e.g. /task/3F2504E0-4F89-42D3-9A0C-0305E82C3301)
        /// </summary>
        public string entityId { get; set; }
        /// <summary>
        /// Relation name to retrieve
        /// </summary>
        public string relationName { get; set; }
        /// <summary>
        /// A list of field names to retrieve
        /// </summary>
        public string[] fields { get; set; }
        /// <summary>
        /// Optionaly order the result
        /// </summary>
        public orderBy[] orders { get; set; }
        /// <summary>
        /// The query criteria
        /// </summary>
        public ICondition where { get; set; }
        /// <summary>
        /// The query relations
        /// </summary>
        public relation[] relations { get; set; }
        /// <summary>
        /// The query result of link objects
        /// </summary>
        public bool fromLink { get; set; }
        /// <summary>
        /// Paging setting for the query
        /// </summary>
        public paging paging { get; set; }

        public relationQuery(string entityId, string relationName, string[] fields, orderBy[] orders, ICondition where, relation[] relations, bool fromLink, paging paging)
        {
            this.entityId = entityId;
            this.relationName = relationName;
            this.fields = fields;
            this.orders = orders;
            this.where = where;
            this.relations = relations;
            this.fromLink = fromLink;
            this.paging = paging;
        }

        // TODO Not working with these fields only
        public relationQuery(string entityId, string relationName, string[] fields)
        {
            this.entityId = entityId;
            this.relationName = relationName;
            this.fields = fields;
        }
    }
}