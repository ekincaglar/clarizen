using Ekin.Clarizen.Interfaces;

namespace Ekin.Clarizen.Data.Queries.Expressions
{
    /// <summary>
    /// An expression which represents an object field 
    /// </summary>
    public class FieldExpression : IExpression
    {
        public string _type { get { return "fieldExpression"; } }

        /// <summary>
        /// The name of the field this expression represents (e.g. "Name","PercentCompleted","Manager" etc.)
        /// </summary>
        public string FieldName { get; set; }

        public FieldExpression(string fieldName)
        {
            FieldName = fieldName;
        }
    }
}