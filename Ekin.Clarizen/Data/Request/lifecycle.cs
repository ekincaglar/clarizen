namespace Ekin.Clarizen.Data.Request
{
    public class Lifecycle
    {
        /// <summary>
        /// A list of objects (Entity Ids) to perform the operation on
        /// </summary>
        public string[] Ids { get; set; }

        /// <summary>
        /// The operation to perform ('Activate', 'Cancel' etc.)
        /// </summary>
        public string Operation { get; set; }

        public Lifecycle(string[] ids, string operation)
        {
            Ids = ids;
            Operation = operation;
        }
    }
}