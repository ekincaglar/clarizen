namespace Ekin.Clarizen.Metadata.Request
{
    public class Objects_put
    {
        /// <summary>
        /// The entity where the workflow rule is created (e.g. WorkItem , User etc.)
        /// </summary>
        public string ForType { get; set; }

        /// <summary>
        /// The name of the workflow rule
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The description of the workflow rule
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The type of event that trigger the workflow rule
        /// Possible values: Create | CreateOrEdit | Delete | CreateOrEditWithPreviousValueNotEqual
        /// </summary>
        public string TriggerType { get; set; }

        /// <summary>
        /// The condition for the workflow rule triggering
        /// </summary>
        public string Criteria { get; set; }

        /// <summary>
        /// The workflow action
        /// </summary>
        public Action Action { get; set; }

        public Objects_put(string forType, string name, string description, string triggerType, string criteria, Action action)
        {
            ForType = forType;
            Name = name;
            Description = description;
            TriggerType = triggerType;
            Criteria = criteria;
            Action = action;
        }
    }
}