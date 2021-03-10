using Ekin.Clarizen.Interfaces;

namespace Ekin.Clarizen.Data.Queries
{
    public class WorkitemRecursiveQuery
    {
        /// <summary>
        /// includeShortcuts
        /// </summary>
        public bool IncludeShortcuts { get; set; }

        /// <summary>
        /// The main entity type to query (e.g. WorkItem, Project, User etc.)
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// The query criteria
        /// </summary>
        public ICondition Where { get; set; }

        /// <summary>
        /// Entity Id
        /// </summary>
        public EntityId RootItemId { get; set; }

        public int Levels { get; set; }

        /// <summary>
        /// A list of field names to retrieve
        /// </summary>
        public string[] Fields { get; set; }

        /// <summary>
        /// Optionally order the result
        /// </summary>
        public OrderBy[] Orders { get; set; }

        /// <summary>
        /// Paging setting for the query
        /// </summary>
        public Paging Paging { get; set; }

        public WorkitemRecursiveQuery()
        {
        }
    }
}