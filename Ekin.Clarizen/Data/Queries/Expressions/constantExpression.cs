using Ekin.Clarizen.Interfaces;

namespace Ekin.Clarizen.Data.Queries.Expressions
{
    /// <summary>
    /// An expression which represents a constant value (e.g. 1, 'Joe', true, null etc.) 
    /// </summary>
    public class ConstantExpression : IExpression
    {
        public string _type { get { return "constantExpression"; } }

        /// <summary>
        /// The expression value
        /// </summary>
        public object Value { get; set; }

        public ConstantExpression(object value)
        {
            Value = value;
        }
    }
}