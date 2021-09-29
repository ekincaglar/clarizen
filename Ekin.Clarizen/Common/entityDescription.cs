using System;

namespace Ekin.Clarizen
{
    public class EntityDescription
    {
        public string TypeName { get; set; }
        public FieldDescription[] Fields { get; set; }
        public string[] ValidStates { get; set; }
        public string Label { get; set; }
        public string LabelPlural { get; set; }
        public string ParentEntity { get; set; }
        public string DisplayField { get; set; }
        public bool Disabled { get; set; }
        public RelationDescription[] Relations { get; set; }
        public PickupDescription[] Pickups { get; set; }
        public EntityDescription() { }

        public void SortFields()
        {
            Array.Sort(Fields, delegate (FieldDescription x, FieldDescription y) { return x.Name.CompareTo(y.Name); });
        }
    }
}