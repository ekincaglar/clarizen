using Ekin.Clarizen.Interfaces;

namespace Ekin.Clarizen.Data.Queries
{
    public class aggregateQuery : IQuery
    {
        public string _type { get { return "aggregateQuery"; } }

        /// <summary>
        /// The main entity type to query (e.g. WorkItem , User etc.)
        /// </summary>
        public string typeName { get; set; }

        /// <summary>
        /// A list of field names to group results by
        /// </summary>
        public string[] groupBy { get; set; }

        /// <summary>
        /// Optionaly order the result
        /// </summary>
        public orderBy[] orders { get; set; }

        /// <summary>
        /// The query criteria
        /// </summary>
        public ICondition where { get; set; }

        /// <summary>
        /// List of aggregations to perform
        /// </summary>
        public fieldAggregation[] aggregations { get; set; }

        /// <summary>
        /// Paging setting for the query
        /// </summary>
        public paging paging { get; set; }

        public aggregateQuery(string typeName, string[] groupBy, orderBy[] orders, ICondition where, fieldAggregation[] aggregations, paging paging)
        {
            this.typeName = typeName;
            this.groupBy = groupBy;
            this.orders = orders;
            this.where = where;
            this.aggregations = aggregations;
            this.paging = paging;
        }

        public aggregateQuery(string typeName, fieldAggregation[] aggregations, string[] groupBy, orderBy[] orders)
        {
            this.typeName = typeName;
            this.aggregations = aggregations;
            this.groupBy = groupBy;
            this.orders = orders;
        }
    }
}