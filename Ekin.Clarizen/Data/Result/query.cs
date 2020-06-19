using System.Linq;

namespace Ekin.Clarizen.Data.Result
{
    public class Query
    {
        /// <summary>
        /// Array of entities returned from this query
        /// </summary>
        public dynamic[] Entities { get; set; }

        /// <summary>
        /// Paging information returned from this query. If paging.hasMore is true, this object should be passed as is, to the same query API in order to retrieve the next page
        /// </summary>
        public Paging Paging { get; set; }

        public string[] GetEntityIds()
        {
            if (Entities != null)
            {
                int entityCount = Entities.Count();
                if (entityCount > 0)
                {
                    string[] ret = new string[entityCount];
                    for (int x = 0; x < entityCount; x++)
                    {
                        ret[x] = Entities[x].id;
                    }
                    return ret;
                }
            }
            return new string[] { };
        }
    }
}