using System.Collections.Generic;

namespace Ekin.Clarizen
{
    public class GetAllResult
    {
        public object Data { get; set; }
        public List<Error> Errors { get; set; }

        public GetAllResult()
        {
            this.Errors = new List<Error>() { };
        }
    }
}