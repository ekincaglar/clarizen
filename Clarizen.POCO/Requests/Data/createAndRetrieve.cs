namespace Ekin.Clarizen.Data.Request
{
    public class createAndRetrieve
    {
        public object entity { get; set; }
        public string[] fields { get; set; }

        public createAndRetrieve(object entity, string[] fields)
        {
            this.entity = entity;
            this.fields = fields;
        }
    }
}