using Ekin.Clarizen.Interfaces;

namespace Ekin.Clarizen.Data.Request
{
    public class CountQuery
    {
        public IQuery Query { get; set; }

        public CountQuery(IQuery query)
        {
            Query = query;
        }
    }
}