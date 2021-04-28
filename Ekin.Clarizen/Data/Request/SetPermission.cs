namespace Ekin.Clarizen.Data.Request
{
    public class SetPermission
    {
        public string EntityId { get; set; }
        public bool Replace { get; set; } = false;
        public permission_set_roles[] Roles { get; set; }

        public SetPermission(string EntityId, permission_set_roles[] Roles, bool Replace = false)
        {
            this.EntityId = EntityId;
            this.Roles = Roles;
            this.Replace = Replace;
        }
    }

    public class permission_set_roles
    {
        public string Role { get; set; }
        public string Resource { get; set; }
    }
}
