using Ekin.Clarizen.Interfaces;

namespace Ekin.Clarizen.Data.Queries
{
    public class ExpenseQuery
    {
        public string ProjectId { get; set; }
        public string CustomerId { get; set; }
        public string TypeName { get; set; }
        public string[] Fields { get; set; }
        public OrderBy[] Orders { get; set; }
        public ICondition Where { get; set; }
        public Relation[] Relations { get; set; }
        public bool Deleted { get; set; }
        public bool OriginalExternalID { get; set; }
        public Paging Paging { get; set; }

        public ExpenseQuery(string projectId, string customerId, string typeName, string[] fields, OrderBy[] orders, ICondition where, Relation[] relations, bool deleted, bool originalExternalID, Paging paging)
        {
            ProjectId = projectId;
            CustomerId = customerId;
            TypeName = typeName;
            Fields = fields;
            Orders = orders;
            Where = where;
            Relations = relations;
            Deleted = deleted;
            OriginalExternalID = originalExternalID;
            Paging = paging;
        }

        public ExpenseQuery()
        {

        }
    }
}