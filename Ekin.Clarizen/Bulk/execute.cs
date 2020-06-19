namespace Ekin.Clarizen.Bulk
{
    public class Execute : Call<Result.Execute>
    {
        public Execute(Request.Execute request, CallSettings callSettings)
        {
            _request = request;

            _callSettings = callSettings;
            _callSettings.IsBulk = false; // Force this call to be made as a single call
            if (request.Batch != null)
            {
                _callSettings.Headers = new System.Net.WebHeaderCollection();
                _callSettings.Headers.Add("CallOptions", string.Format("Batch={0}", request.Batch.GetValueOrDefault() ? "true" : "false"));
            }

            _url = callSettings.ServerLocation + "/bulk/execute";
            _method = RequestMethod.Post;
        }
    }
}