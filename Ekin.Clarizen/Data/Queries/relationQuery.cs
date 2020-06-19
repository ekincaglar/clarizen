using Ekin.Clarizen.Interfaces;

namespace Ekin.Clarizen.Data.Queries
{
    public class RelationQuery
    {
        /// <summary>
        /// Represents the unique Id of an entity in Clarizen
        /// Format: /typeName/entityId (e.g. /task/3F2504E0-4F89-42D3-9A0C-0305E82C3301)
        /// </summary>
        public string EntityId { get; set; }

        /// <summary>
        /// Relation name to retrieve
        /// </summary>
        public string RelationName { get; set; }

        /// <summary>
        /// A list of field names to retrieve
        /// </summary>
        public string[] Fields { get; set; }

        /// <summary>
        /// Optionaly order the result
        /// </summary>
        public OrderBy[] Orders { get; set; }

        /// <summary>
        /// The query criteria
        /// </summary>
        public ICondition Where { get; set; }

        /// <summary>
        /// The query relations
        /// </summary>
        public Relation[] Relations { get; set; }

        /// <summary>
        /// The query result of link objects
        /// </summary>
        public bool FromLink { get; set; }

        /// <summary>
        /// Paging setting for the query
        /// </summary>
        public Paging Paging { get; set; }

        public RelationQuery(string entityId, string relationName, string[] fields, OrderBy[] orders, ICondition where, Relation[] relations, bool fromLink, Paging paging)
        {
            EntityId = entityId;
            RelationName = relationName;
            Fields = fields;
            Orders = orders;
            Where = where;
            Relations = relations;
            FromLink = fromLink;
            Paging = paging;
        }

        public RelationQuery(string entityId, string relationName, string[] fields)
        {
            EntityId = entityId;
            RelationName = relationName;
            Fields = fields;
        }
    }
}