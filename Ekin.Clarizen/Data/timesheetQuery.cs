namespace Ekin.Clarizen.Data
{
    public class TimesheetQuery : Call<Result.TimesheetQuery>
    {
        public TimesheetQuery(Queries.TimesheetQuery request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.IsBulk ? string.Empty : callSettings.ServerLocation) + "/data/timesheetQuery";
            _method = RequestMethod.Post;
        }
    }
}