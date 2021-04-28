namespace Ekin.Clarizen.Data.Request
{
    public class DeletePermission
    {
        public string EntityId { get; set; }
        public string[] Resource { get; set; }

        public DeletePermission(string EntityId, string[] Resource)
        {
            this.EntityId = EntityId;
            this.Resource = Resource;
        }
    }
}
