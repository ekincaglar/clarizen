using Ekin.Clarizen.Interfaces;

namespace Ekin.Clarizen.Data.Queries
{
    public class GroupsQuery : IQuery
    {
        public string _type { get { return "groupsQuery"; } }

        public string[] Fields { get; set; }
        public Paging Paging { get; set; }

        public GroupsQuery(string[] fields, Paging paging)
        {
            Fields = fields;
            Paging = paging;
        }

        public GroupsQuery(string[] fields)
        {
            Fields = fields;
        }
    }
}