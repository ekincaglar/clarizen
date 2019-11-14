using Ekin.Clarizen.Interfaces;

namespace Ekin.Clarizen.Data.Queries.Conditions
{
    public class And : ICondition
    {
        public string _type { get { return "and"; } }
        public ICondition[] and { get; set; }

        public And(ICondition[] and)
        {
            this.and = and;
        }
    }
}