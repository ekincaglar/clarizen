namespace Ekin.Clarizen.Data.Request
{
    public class CreateAndRetrieve
    {
        public object Entity { get; set; }
        public string[] Fields { get; set; }

        public CreateAndRetrieve(object entity, string[] fields)
        {
            Entity = entity;
            Fields = fields;
        }
    }
}