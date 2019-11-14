namespace Ekin.Clarizen.Data.Request
{
    public class objects_post
    {
        public object entity { get; set; }

        public objects_post(object entity)
        {
            this.entity = entity;
        }
    }
}