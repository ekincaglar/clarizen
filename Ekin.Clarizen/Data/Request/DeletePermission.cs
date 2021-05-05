namespace Ekin.Clarizen.Data.Request
{
    public class DeletePermission
    {
        /// <summary>
        /// Id of the entity
        /// </summary>
        public string EntityId { get; set; }

        /// <summary>
        /// Resources to delete
        /// </summary>
        public string[] Resource { get; set; }

        public DeletePermission(string EntityId, string[] Resource)
        {
            this.EntityId = EntityId;
            this.Resource = Resource;
        }
    }
}
