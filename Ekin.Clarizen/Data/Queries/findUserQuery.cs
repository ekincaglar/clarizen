using Ekin.Clarizen.Interfaces;

namespace Ekin.Clarizen.Data.Queries
{
    /// <summary>
    /// Finds a user based on several criterias 
    /// </summary>
    public class FindUserQuery : IQuery
    {
        public string _type { get { return "findUserQuery"; } }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool FuzzySearchUserName { get; set; }
        public bool IncludeSuspendedUsers { get; set; }
        public string[] Fields { get; set; }
        public OrderBy[] Orders { get; set; }
        public Paging Paging { get; set; }

        public FindUserQuery(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }
}