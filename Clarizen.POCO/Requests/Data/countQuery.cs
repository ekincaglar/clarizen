using Ekin.Clarizen.Interfaces;

namespace Ekin.Clarizen.Data.Request
{
    public class countQuery
    {
        public IQuery query { get; set; }

        public countQuery(IQuery query)
        {
            this.query = query;
        }
    }
}