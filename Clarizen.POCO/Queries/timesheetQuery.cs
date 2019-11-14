using Ekin.Clarizen.Interfaces;

namespace Ekin.Clarizen.Data.Queries
{
    public class timesheetQuery
    {
        /// <summary>
        /// Entity Id
        /// </summary>
        public string projectId { get; set; }

        /// <summary>
        /// Entity Id
        /// </summary>
        public string customerId { get; set; }

        public bool iAmTheApprover { get; set; }

        /// <summary>
        /// Array of entity Ids
        /// </summary>
        public string[] workItems { get; set; }

        /// <summary>
        /// UnSubmitted | PendingApproval | Approved | All
        /// </summary>
        public string timesheetState { get; set; }

        /// <summary>
        /// The main entity type to query (e.g. WorkItem, User etc.)
        /// </summary>
        public string typeName { get; set; }

        /// <summary>
        /// A list of field names to retrieve
        /// </summary>
        public string[] fields { get; set; }

        /// <summary>
        /// Optionaly order the result
        /// </summary>
        public orderBy[] orders { get; set; }

        /// <summary>
        /// The query criteria
        /// </summary>
        public ICondition where { get; set; }

        /// <summary>
        /// The query relations
        /// </summary>
        public relation[] relations { get; set; }

        /// <summary>
        /// If set to true, the query is performed on Deleted entities
        /// </summary>
        public bool deleted { get; set; }

        public bool originalExternalID { get; set; }

        /// <summary>
        /// Paging setting for the query
        /// </summary>
        public paging paging { get; set; }

        public timesheetQuery()
        {
        }
    }
}