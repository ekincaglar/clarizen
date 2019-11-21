namespace Ekin.Clarizen.Tests.Models
{
    public class User
    {
        public bool Admin { get; set; }
        public string DirectManager { get; set; }
        public string email { get; set; }
        public bool ExternalUser { get; set; }
        public bool FinancialPermissions { get; set; }
        public string FirstName { get; set; }
        public string JobTitle { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public string OfficerPhone { get; set; }
        public bool SuperUser { get; set; }
        public string UserName { get; set; }
    }
}