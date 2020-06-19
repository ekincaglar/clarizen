using Ekin.Clarizen.Interfaces;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace Ekin.Clarizen.Data.Queries.Conditions
{
    public class Compare : ICondition
    {
        public string _type { get { return "compare"; } }

        public IExpression LeftExpression { get; set; }

        [XmlElement("operator")]
        [JsonProperty("operator")]
        public string Operator { get; set; }

        public IExpression RightExpression { get; set; }

        public Compare(IExpression leftExpression, Operator Operator, IExpression rightExpression)
        {
            LeftExpression = leftExpression;
            this.Operator = Operator.ToEnumString();
            RightExpression = rightExpression;
        }

    }
}