using Ekin.Clarizen.Interfaces;
using Ekin.Clarizen.Data.Queries.Filters;

namespace Ekin.Clarizen.Data.Queries
{
    public class CrossOrgEntityQuery : IQuery
    {
        public string _type { get { return "crossOrgEntityQuery"; } }

        public string TypeName { get; set; }
        public UserDateFilter OrganizationFilter { get; set; }
        public string[] Fields { get; set; }
        public OrderBy[] Orders { get; set; }
        public ICondition Where { get; set; }
        public Relation[] Relations { get; set; }
        public bool Deleted { get; set; }
        public bool OriginalExternalID { get; set; }
        public Paging Paging { get; set; }

        public CrossOrgEntityQuery(string typeName)
        {
            TypeName = typeName;
        }

        public CrossOrgEntityQuery(string typeName, UserDateFilter organizationFilter, string[] fields)
        {
            TypeName = typeName;
            Fields = fields;
            OrganizationFilter = organizationFilter;
        }

        public CrossOrgEntityQuery(string typeName, UserDateFilter organizationFilter, string[] fields, ICondition where, Paging paging)
        {
            TypeName = typeName;
            OrganizationFilter = organizationFilter;
            Fields = fields;
            Where = where;
            Paging = paging;
        }

        public CrossOrgEntityQuery(string typeName, UserDateFilter organizationFilter, string[] fields, OrderBy[] orders, ICondition where, Relation[] relations, bool deleted, bool originalExternalID, Paging paging)
        {
            TypeName = typeName;
            OrganizationFilter = organizationFilter;
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