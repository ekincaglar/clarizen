namespace Ekin.Clarizen.Data.Request
{
    public class GetTemplateDescriptions
    {
        public string TypeName { get; set; }

        public GetTemplateDescriptions(string typeName)
        {
            TypeName = typeName;
        }
    }
}