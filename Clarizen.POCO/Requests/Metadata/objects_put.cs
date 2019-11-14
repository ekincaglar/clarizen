namespace Ekin.Clarizen.Metadata.Request
{
    public class objects_put
    {
        /// <summary>
        /// The entity where the workflow rule is created (e.g. WorkItem , User etc.)
        /// </summary>
        public string forType { get; set; }

        /// <summary>
        /// The name of the workflow rule
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// The description of the workflow rule
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// The type of event that trigger the workflow rule
        /// Possible values: Create | CreateOrEdit | Delete | CreateOrEditWithPreviousValueNotEqual
        /// </summary>
        public string triggerType { get; set; }

        /// <summary>
        /// The condition for the workflow rule triggering
        /// </summary>
        public string criteria { get; set; }

        /// <summary>
        /// The workflow action
        /// </summary>
        public action action { get; set; }

        public objects_put(string forType, string name, string description, string triggerType, string criteria, action action)
        {
            this.forType = forType;
            this.name = name;
            this.description = description;
            this.triggerType = triggerType;
            this.criteria = criteria;
            this.action = action;
        }
    }
}