namespace Ekin.Clarizen.Data.Request
{
    public class createFromTemplate
    {
        /// <summary>
        /// Entity to be created
        /// </summary>
        public object entity { get; set; }

        /// <summary>
        /// Name of the template to use
        /// </summary>
        public string templateName { get; set; }

        /// <summary>
        /// Entity Id of the parent
        /// </summary>
        public string parentId { get; set; }

        public createFromTemplate(object entity, string templateName, string parentId)
        {
            this.entity = entity;
            this.templateName = templateName;
            this.parentId = parentId;
        }
    }
}