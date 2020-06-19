using Ekin.Clarizen.Interfaces;

namespace Ekin.Clarizen.Data.Queries
{
    public class EntityQuery : IQuery
    {
        public string _type { get { return "entityQuery"; } }

        public string TypeName { get; set; }
        public string[] Fields { get; set; }
        public OrderBy[] Orders { get; set; }
        public ICondition Where { get; set; }
        public Relation[] Relations { get; set; }
        public bool Deleted { get; set; }
        public bool OriginalExternalID { get; set; }
        public Paging Paging { get; set; }

        public EntityQuery(string typeName)
        {
            TypeName = typeName;
        }

        public EntityQuery(string typeName, string[] fields)
        {
            TypeName = typeName;
            Fields = fields;
        }

        public EntityQuery(string typeName, string[] fields, OrderBy[] orders, ICondition where, Relation[] relations, bool deleted, bool originalExternalID, Paging paging)
        {
            TypeName = typeName;
            Fields = fields;
            Orders = orders;
            Where = where;
            Relations = relations;
            Deleted = deleted;
            OriginalExternalID = originalExternalID;
            Paging = paging;
        }
    }
}