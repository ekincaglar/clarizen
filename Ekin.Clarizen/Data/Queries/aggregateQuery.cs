using Ekin.Clarizen.Interfaces;

namespace Ekin.Clarizen.Data.Queries
{
    public class AggregateQuery : IQuery
    {
        public string _type { get { return "aggregateQuery"; } }

        /// <summary>
        /// The main entity type to query (e.g. WorkItem , User etc.)
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// A list of field names to group results by
        /// </summary>
        public string[] GroupBy { get; set; }
        /// <summary>
        /// Optionaly order the result
        /// </summary>
        public OrderBy[] Orders { get; set; }
        /// <summary>
        /// The query criteria
        /// </summary>
        public ICondition Where { get; set; }
        /// <summary>
        /// List of aggregations to perform
        /// </summary>
        public FieldAggregation[] Aggregations { get; set; }
        /// <summary>
        /// Paging setting for the query
        /// </summary>
        public Paging Paging { get; set; }

        public AggregateQuery(string typeName, string[] groupBy, OrderBy[] orders, ICondition where, FieldAggregation[] aggregations, Paging paging)
        {
            TypeName = typeName;
            GroupBy = groupBy;
            Orders = orders;
            Where = where;
            Aggregations = aggregations;
            Paging = paging;
        }

        public AggregateQuery(string typeName, FieldAggregation[] aggregations, string[] groupBy, OrderBy[] orders)
        {
            TypeName = typeName;
            Aggregations = aggregations;
            GroupBy = groupBy;
            Orders = orders;
        }
    }
}