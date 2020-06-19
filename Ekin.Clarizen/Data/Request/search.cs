namespace Ekin.Clarizen.Data.Request
{
    public class Search
    {
        /// <summary>
        /// The search query to perform
        /// </summary>
        public string Q { get; set; }
        /// <summary>
        /// (Optional) The Entity Type to search. If omitted, search on all types
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// The list of fields to return. Only valid when specifying a TypeName. When searching in all types the fields returned are fixed
        /// </summary>
        public string[] Fields { get; set; }
        /// <summary>
        /// Paging setting for the query
        /// </summary>
        public Paging Paging { get; set; }

        public Search(string q, string typeName, string[] fields, Paging paging)
        {
            Q = q;
            TypeName = typeName;
            Fields = fields;
            Paging = paging;
        }

        public Search(string q, string typeName, string[] fields)
        {
            Q = q;
            TypeName = typeName;
            Fields = fields;
        }

        public Search(string q)
        {
            Q = q;
        }

        public Search(string q, Paging paging)
        {
            Q = q;
            Paging = paging;
        }

    }
}