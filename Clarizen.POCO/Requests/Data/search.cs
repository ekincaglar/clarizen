namespace Ekin.Clarizen.Data.Request
{
    public class search
    {
        /// <summary>
        /// The search query to perform
        /// </summary>
        public string q { get; set; }

        /// <summary>
        /// (Optional) The Entity Type to search. If omitted, search on all types
        /// </summary>
        public string typeName { get; set; }

        /// <summary>
        /// The list of fields to return. Only valid when specifying a TypeName. When searching in all types the fields returned are fixed
        /// </summary>
        public string[] fields { get; set; }

        /// <summary>
        /// Paging setting for the query
        /// </summary>
        public paging paging { get; set; }

        public search(string q, string typeName, string[] fields, paging paging)
        {
            this.q = q;
            this.typeName = typeName;
            this.fields = fields;
            this.paging = paging;
        }

        public search(string q, string typeName, string[] fields)
        {
            this.q = q;
            this.typeName = typeName;
            this.fields = fields;
        }

        public search(string q)
        {
            this.q = q;
        }

        public search(string q, paging paging)
        {
            this.q = q;
            this.paging = paging;
        }
    }
}