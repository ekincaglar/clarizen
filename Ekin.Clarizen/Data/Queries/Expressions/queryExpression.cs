using Ekin.Clarizen.Interfaces;

namespace Ekin.Clarizen.Data.Queries.Expressions
{
    /// <summary>
    /// An expression which represents a query. Used in IN conditions 
    /// </summary>
    public class QueryExpression : IExpression
    {
        public string _type { get { return "queryExpression"; } }

        /// <summary>
        /// The actual query
        /// </summary>
        public IQuery Query { get; set; }

        public QueryExpression(IQuery query)
        {
            Query = query;
        }
    }
}