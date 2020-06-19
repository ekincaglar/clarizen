namespace Ekin.Clarizen.Data.Request
{
    public class ExecuteCustomAction
    {
        public string TargetId { get; set; }
        public string CustomAction { get; set; }
        public FieldValue[] Values { get; set; }

        public ExecuteCustomAction(string targetId, string customAction, FieldValue[] values)
        {
            TargetId = targetId;
            CustomAction = customAction;
            Values = values;
        }
    }
}