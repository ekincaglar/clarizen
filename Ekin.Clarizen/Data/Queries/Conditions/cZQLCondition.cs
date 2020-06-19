using Ekin.Clarizen.Interfaces;

namespace Ekin.Clarizen.Data.Queries.Conditions
{
    public class CZQLCondition : ICondition
    {
        public string _type { get { return "cZQLCondition"; } }

        /// <summary>
        /// The condition text (e.g. PercentCompleted>50 AND DueDate>:dateParam) You can use bind parameters in the condition text and fill the parameter values using Parameters property
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Values for bind parameters
        /// </summary>
        public Parameters Parameters { get; set; }

        public CZQLCondition(string text, Parameters parameters)
        {
            Text = text;
            Parameters = parameters;
        }

        public CZQLCondition(string text)
        {
            Text = text;
        }
    }
}