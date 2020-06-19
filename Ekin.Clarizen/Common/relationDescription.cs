namespace Ekin.Clarizen
{
    public class RelationDescription
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public string RoleLabel { get; set; }
        public bool ReadOnly { get; set; }
        public string LinkTypeName { get; set; }
        public string RelatedTypeName { get; set; }
        public string SourceFieldName { get; set; }

        public RelationDescription() { }
    }
}