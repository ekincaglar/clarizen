using System;

namespace Ekin.Clarizen
{
    public class EntityRelationsDescription
    {
        public string TypeName { get; set; }
        public RelationDescription[] Relations { get; set; }

        public EntityRelationsDescription() { }

        public void SortRelations()
        {
            Array.Sort(Relations, delegate (RelationDescription x, RelationDescription y) { return x.Name.CompareTo(y.Name); });
        }
    }
}