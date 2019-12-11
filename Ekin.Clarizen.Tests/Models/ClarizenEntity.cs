namespace Ekin.Clarizen.Tests.Models
{
    public class ClarizenEntity
    {
        public string name { get; set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(name) ? base.ToString() : name;
        }
    }
    
}