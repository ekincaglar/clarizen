namespace Ekin.Clarizen.Data
{
    public class Search : Call<Result.Search>
    {
        public Search(Request.Search request, CallSettings callSettings)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Q))
            {
                IsCalledSuccessfully = false;
                this.Error = "Search query must be provided";
                return;
            }

            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.IsBulk ? string.Empty : callSettings.ServerLocation) + "/data/search?q=" + request.Q +
                    (request.Fields != null ? "&" + request.Fields.ToQueryString() : string.Empty) +
                    (!string.IsNullOrWhiteSpace(request.TypeName) ? "&" + request.TypeName.ToQueryString() : string.Empty) +
                    (request.Paging != null ? "&" + request.Paging.ToQueryString() : string.Empty);
            _method = System.Net.Http.HttpMethod.Get;
        }
    }
}