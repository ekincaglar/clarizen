namespace Ekin.Clarizen
{
    public class FieldValue
    {
        public string FieldName { get; set; }
        public object Value { get; set; }

        public FieldValue() { }

        public FieldValue(string fieldName, object value)
        {
            FieldName = fieldName;
            Value = value;
        }
    }
}