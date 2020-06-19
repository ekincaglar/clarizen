namespace Ekin.Clarizen.Data.Request
{
    public class Query
    {
        /// <summary>
        /// The CZQL Query to perform
        /// </summary>
        public string Q { get; set; }
        /// <summary>
        /// Values for bind parameters
        /// </summary>
        public object[] Parameters { get; set; }
        /// <summary>
        /// paging setting for the query
        /// </summary>
        public Paging Paging { get; set; }

        public Query(string q, object[] parameters, Paging paging)
        {
            Q = q;
            Parameters = parameters;
            Paging = paging;
        }

        public Query(string q, Paging paging)
        {
            Q = q;
            Paging = paging;
        }

        public Query(string q)
        {
            Q = q;
        }

    }
}