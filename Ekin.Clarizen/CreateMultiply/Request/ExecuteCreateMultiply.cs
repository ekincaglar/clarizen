namespace Ekin.Clarizen.CreateMultiply.Request
{
    public class ExecuteCreateMultiply
    {
        /// <summary>
        /// Array of Request objects representing individual API calls
        /// </summary>
        public object RequestBody { get; set; }
        public bool Transactional { get; set; }
        public bool IsMultiply { get; set; } = false;
        public string CallOptions { get; set; }

        public ExecuteCreateMultiply(object requests, bool transactional, bool IsMultiply,string callOptions)
        {
            this.RequestBody = requests;
            this.Transactional = transactional;
            this.IsMultiply = IsMultiply;
            this.CallOptions = callOptions;
        }
    }
}
