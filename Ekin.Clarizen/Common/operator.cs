namespace Ekin.Clarizen
{
    public enum Operator
    {
        /// <summary>
        /// =
        /// </summary>
        Equal,
        /// <summary>
        /// <>
        /// </summary>
        NotEqual,
        /// <summary>
        /// <
        /// </summary>
        LessThan,
        /// <summary>
        /// >
        /// </summary>
        GreaterThan,
        /// <summary>
        /// <=
        /// </summary>
        LessThanOrEqual,
        /// <summary>
        /// >=
        /// </summary>
        GreaterThanOrEqual,
        /// <summary>
        /// Checks whether a string starts with another string
        /// </summary>
        BeginsWith,
        /// <summary>
        /// Checks whether a string ends with another string
        /// </summary>
        EndsWith,
        /// <summary>
        /// Checks whether a string contains another string
        /// </summary>
        Contains,
        /// <summary>
        /// Checks whether a string does not contain with another string
        /// </summary>
        DoesNotContain,
        /// <summary>
        /// The In operator is supported in 2 scenarios: 1. Where the left side is a FieldExpression which represents a "reference to entity" field and the right side is a QueryExpression 2. Where the left side is a FieldExpression which represents a simple value and the right side is a ConstantListExpression which represents a list of values
        /// </summary>
        In,
        /// <summary>
        /// Performs an SQL LIKE on a field (e.g. Name like '%joe%')
        /// </summary>
        Like
    }

    public static class OperatorExtensions
    {
        public static string ToEnumString(this Operator me)
        {
            switch (me)
            {
                case Operator.BeginsWith: return "BeginsWith";
                case Operator.Contains: return "Contains";
                case Operator.DoesNotContain: return "DoesNotContain";
                case Operator.EndsWith: return "EndsWith";
                case Operator.Equal: return "Equal";
                case Operator.GreaterThan: return "GreaterThan";
                case Operator.GreaterThanOrEqual: return "GreaterThanOrEqual";
                case Operator.In: return "In";
                case Operator.LessThan: return "LessThan";
                case Operator.LessThanOrEqual: return "LessThanOrEqual";
                case Operator.Like: return "Like";
                case Operator.NotEqual: return "NotEqual";
                default: return "ERROR";
            }
        }
    }
}