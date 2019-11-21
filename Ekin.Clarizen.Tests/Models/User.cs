namespace Ekin.Clarizen.Tests.Models
{
    public class User
    {
        public string DirectManager { get; set; }
        public string email { get; set; }
        public bool ExternalUser { get; set; }
        public bool Financial { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobilePhone { get; set; }
        public string OfficePhone { get; set; }
        public bool SuperUser { get; set; }
        public string UserName { get; set; }
        public string Id { get;  }
    }
}