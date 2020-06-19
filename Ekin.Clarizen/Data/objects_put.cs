namespace Ekin.Clarizen.Data
{
    public class Objects_put : Call<Result.Objects_put>
    {
        public Objects_put(string id, object obj, CallSettings callSettings)
        {
            _request = obj;
            _callSettings = callSettings;
            _url = (callSettings.IsBulk ? string.Empty : callSettings.ServerLocation) + "/data/objects" +
                         (id.Substring(0, 1) != "/" ? "/" : "") + id;
            _method = RequestMethod.Put;
        }
    }
}