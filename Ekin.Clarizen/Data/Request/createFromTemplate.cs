namespace Ekin.Clarizen.Data.Request
{
    public class CreateFromTemplate
    {
        /// <summary>
        /// Entity to be created
        /// </summary>
        public object Entity { get; set; }
        /// <summary>
        /// Name of the template to use
        /// </summary>
        public string TemplateName { get; set; }
        /// <summary>
        /// Entity Id of the parent
        /// </summary>
        public string ParentId { get; set; }

        public CreateFromTemplate(object entity, string templateName, string parentId)
        {
            Entity = entity;
            TemplateName = templateName;
            ParentId = parentId;
        }
    }
}