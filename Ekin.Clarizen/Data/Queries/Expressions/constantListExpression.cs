using Ekin.Clarizen.Interfaces;

namespace Ekin.Clarizen.Data.Queries.Expressions
{
    /// <summary>
    /// An expression which represents array of constant values. Used in IN conditions 
    /// </summary>
    public class ConstantListExpression : IExpression
    {
        public string _type { get { return "constantListExpression"; } }

        /// <summary>
        /// The value list
        /// </summary>
        public object[] Value { get; set; }

        public ConstantListExpression(object[] value)
        {
            Value = value;
        }
    }
}