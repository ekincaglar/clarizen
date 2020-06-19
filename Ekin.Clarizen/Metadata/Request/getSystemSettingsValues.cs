namespace Ekin.Clarizen.Metadata.Request
{
    public class GetSystemSettingsValues
    {
        /// <summary>
        /// List of system setting names
        /// </summary>
        public string[] Settings { get; set; }

        public GetSystemSettingsValues(string[] settings)
        {
            Settings = settings;
        }
    }
}