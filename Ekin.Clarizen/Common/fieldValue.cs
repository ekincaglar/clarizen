namespace Ekin.Clarizen
{
    public class FieldValue
    {
        public string FieldName { get; set; }
        public string Value { get; set; }

        public FieldValue() { }

        public FieldValue(string fieldName, string value)
        {
            FieldName = fieldName;
            Value = value;
        }
    }
}