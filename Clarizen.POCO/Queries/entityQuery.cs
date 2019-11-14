using Ekin.Clarizen.Interfaces;

namespace Ekin.Clarizen.Data.Queries
{
    public class entityQuery : IQuery
    {
        public string _type { get { return "entityQuery"; } }

        public string typeName { get; set; }
        public string[] fields { get; set; }
        public orderBy[] orders { get; set; }
        public ICondition where { get; set; }
        public relation[] relations { get; set; }   // NOT TESTED
        public bool deleted { get; set; }
        public bool originalExternalID { get; set; }
        public paging paging { get; set; }

        public entityQuery(string typeName)
        {
            this.typeName = typeName;
        }

        public entityQuery(string typeName, string[] fields)
        {
            this.typeName = typeName;
            this.fields = fields;
        }

        public entityQuery(string typeName, string[] fields, orderBy[] orders, ICondition where, relation[] relations, bool deleted, bool originalExternalID, paging paging)
        {
            this.typeName = typeName;
            this.fields = fields;
            this.orders = orders;
            this.relations = relations;
            this.deleted = deleted;
            this.originalExternalID = originalExternalID;
            this.paging = paging;
        }
    }
}