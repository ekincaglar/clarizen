namespace Ekin.Clarizen.Data.Request
{
    public class RetrieveMultiple
    {
        public string[] Fields { get; set; }
        public string[] Ids { get; set; }

        public RetrieveMultiple(string[] fields, string[] ids)
        {
            Fields = fields;
            Ids = ids;
        }
    }
}