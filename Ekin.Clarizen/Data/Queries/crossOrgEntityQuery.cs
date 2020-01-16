using Ekin.Clarizen.Interfaces;
using Ekin.Clarizen.Data.Queries.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen.Data.Queries
{
    public class crossOrgEntityQuery : IQuery
    {
        public string _type { get { return "crossOrgEntityQuery"; } }

        public string typeName { get; set; }
        public UserDateFilter organizationFilter { get; set; }
        public string[] fields { get; set; }
        public orderBy[] orders { get; set; }
        public ICondition where { get; set; }
        public relation[] relations { get; set; }   // NOT TESTED
        public bool deleted { get; set; }
        public bool originalExternalID { get; set; }
        public paging paging { get; set; }

        public crossOrgEntityQuery(string typeName)
        {
            this.typeName = typeName;
        }

        public crossOrgEntityQuery(string typeName, UserDateFilter organizationFilter, string[] fields)
        {
            this.typeName = typeName;
            this.fields = fields;
            this.organizationFilter = organizationFilter;
        }

        public crossOrgEntityQuery(string typeName, UserDateFilter organizationFilter, string[] fields, ICondition where, paging paging)
        {
            this.typeName = typeName;
            this.organizationFilter = organizationFilter;
            this.fields = fields;
            this.where = where;
            this.paging = paging;
        }

        public crossOrgEntityQuery(string typeName, UserDateFilter organizationFilter, string[] fields, orderBy[] orders, ICondition where, relation[] relations, bool deleted, bool originalExternalID, paging paging)
        {
            this.typeName = typeName;
            this.organizationFilter = organizationFilter;
            this.fields = fields;
            this.orders = orders;
            this.where = where;
            this.relations = relations;
            this.deleted = deleted;
            this.originalExternalID = originalExternalID;
            this.paging = paging;
        }
    }
}