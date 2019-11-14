using Ekin.Clarizen.Interfaces;

namespace Ekin.Clarizen.Data.Queries
{
    public class expenseQuery
    {
        public string projectId { get; set; }
        public string customerId { get; set; }
        public string typeName { get; set; }
        public string[] fields { get; set; }
        public orderBy[] orders { get; set; }
        public ICondition where { get; set; }
        public relation[] relations { get; set; }
        public bool deleted { get; set; }
        public bool originalExternalID { get; set; }
        public paging paging { get; set; }

        public expenseQuery(string projectId, string customerId, string typeName, string[] fields, orderBy[] orders, ICondition where, relation[] relations, bool deleted, bool originalExternalID, paging paging)
        {
            this.projectId = projectId;
            this.customerId = customerId;
            this.typeName = typeName;
            this.fields = fields;
            this.orders = orders;
            this.where = where;
            this.relations = relations;
            this.deleted = deleted;
            this.originalExternalID = originalExternalID;
            this.paging = paging;
        }

        public expenseQuery()
        {
        }
    }
}