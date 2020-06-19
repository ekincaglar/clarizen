namespace Ekin.Clarizen.Metadata.Request
{
    public class SetSystemSettingsValues
    {
        /// <summary>
        /// List of system setting values
        /// </summary>
        public FieldValue[] Settings { get; set; }

        public SetSystemSettingsValues(FieldValue[] settings)
        {
            Settings = settings;
        }
    }
}