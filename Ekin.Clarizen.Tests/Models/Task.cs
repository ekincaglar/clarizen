namespace Ekin.Clarizen.Tests.Models
{
    public class Task : BaseModel
    {
        public Task()
        {
        }

        public Task(string id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public Task(string name)
        {
            this.Name = name;
        }
    }
}