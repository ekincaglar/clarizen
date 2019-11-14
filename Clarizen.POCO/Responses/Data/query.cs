using System.Linq;
using System.Dynamic;
namespace Ekin.Clarizen.Data.Result
{
    public class query
    {
        /// <summary>
        /// Array of entities returned from this query
        /// </summary>
        public dynamic[] entities { get; set; }

        /// <summary>
        /// Paging information returned from this query. If paging.hasMore is true, this object should be passed as is, to the same query API in order to retrieve the next page
        /// </summary>
        public paging paging { get; set; }

        public string[] GetEntityIds()
        {
            if (entities == null || !entities.Any()) return new string[] { };
            int entityCount = this.entities.Count();

            string[] ret = new string[entityCount];
            for (int x = 0; x < entityCount; x++)
            {
                ret[x] = this.entities[x].id;
            }
            return ret;
        }
    }
}