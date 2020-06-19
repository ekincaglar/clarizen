namespace Ekin.Clarizen.Metadata.Result
{
    public class GetSystemSettingsValues
    {
        /// <summary>
        /// Array of objects representing the values of each system setting. The value type (e.g. boolean, int etc.) depends on the system setting
        /// </summary>
        public object[] Values { get; set; }
    }
}