namespace Ekin.Clarizen.Data.Request
{
    public class ChangeState
    {
        /// <summary>
        /// List of objects to perform the operation on
        /// EntityID format: /typeName/entityId (e.g. /task/3F2504E0-4F89-42D3-9A0C-0305E82C3301)
        /// </summary>
        public string[] Ids { get; set; }
        /// <summary>
        /// The new state
        /// </summary>
        public string State { get; set; }

        public ChangeState(string[] ids, string state)
        {
            this.Ids = ids;
            this.State = state;
        }
    }
}