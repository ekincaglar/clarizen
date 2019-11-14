namespace Ekin.Clarizen.Data.Request
{
    public class getTemplateDescriptions
    {
        public string typeName { get; set; }

        public getTemplateDescriptions(string typeName)
        {
            this.typeName = typeName;
        }
    }
}