namespace Ekin.Clarizen.Data.Request
{
    public class SetPermission
    {
        /// <summary>
        /// The Id of entity
        /// </summary>
        public string EntityId { get; set; }

        /// <summary>
        /// When replace is true the permission will be overriden
        /// </summary>
        public bool Replace { get; set; } = false;

        /// <summary>
        /// Roles to add
        /// </summary>
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
