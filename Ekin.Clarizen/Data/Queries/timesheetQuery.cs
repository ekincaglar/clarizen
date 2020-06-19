using Ekin.Clarizen.Interfaces;

namespace Ekin.Clarizen.Data.Queries
{
    public class TimesheetQuery
    {
        /// <summary>
        /// Entity Id
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// Entity Id
        /// </summary>
        public string CustomerId { get; set; }

        public bool IAmTheApprover { get; set; }

        /// <summary>
        /// Array of entity Ids
        /// </summary>
        public string[] WorkItems { get; set; }

        /// <summary>
        /// UnSubmitted | PendingApproval | Approved | All
        /// </summary>
        public string TimesheetState { get; set; }

        /// <summary>
        /// The main entity type to query (e.g. WorkItem, User etc.)
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// A list of field names to retrieve
        /// </summary>
        public string[] Fields { get; set; }

        /// <summary>
        /// Optionaly order the result
        /// </summary>
        public OrderBy[] Orders { get; set; }

        /// <summary>
        /// The query criteria
        /// </summary>
        public ICondition Where { get; set; }

        /// <summary>
        /// The query relations
        /// </summary>
        public Relation[] Relations { get; set; }

        /// <summary>
        /// If set to true, the query is performed on Deleted entities
        /// </summary>
        public bool Deleted { get; set; }

        public bool OriginalExternalID { get; set; }

        /// <summary>
        /// Paging setting for the query
        /// </summary>
        public Paging Paging { get; set; }

        public TimesheetQuery()
        {

        }
    }
}