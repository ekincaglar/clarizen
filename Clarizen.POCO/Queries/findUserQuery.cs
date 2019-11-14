using Ekin.Clarizen.Interfaces;

namespace Ekin.Clarizen.Data.Queries
{
    /// <summary>
    /// Finds a user based on several criterias
    /// </summary>
    public class findUserQuery : IQuery
    {
        public string _type { get { return "findUserQuery"; } }

        public string firstName { get; set; }
        public string lastName { get; set; }
        public string eMail { get; set; }
        public bool fuzzySearchUserName { get; set; }
        public bool includeSuspendedUsers { get; set; }
        public string[] fields { get; set; }
        public orderBy[] orders { get; set; }
        public paging paging { get; set; }

        public findUserQuery(string firstName, string lastName)
        {
            this.firstName = firstName;
            this.lastName = lastName;
        }
    }
}