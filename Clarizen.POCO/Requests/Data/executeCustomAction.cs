namespace Ekin.Clarizen.Data.Request
{
    public class executeCustomAction
    {
        public string targetId { get; set; }
        public string customAction { get; set; }
        public fieldValue[] values { get; set; }

        public executeCustomAction(string targetId, string customAction, fieldValue[] values)
        {
            this.targetId = targetId;
            this.customAction = customAction;
            this.values = values;
        }
    }
}