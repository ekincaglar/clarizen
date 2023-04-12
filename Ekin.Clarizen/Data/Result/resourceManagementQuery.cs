using Ekin.Clarizen.Common;
using System;

namespace Ekin.Clarizen.Data.Result
{
    public class ResourceManagementQuery
    {
        public bool ProjectAssignmentIncluded { get; set; }
        public ResourceManagementNodes[] Nodes { get; set; }
    }

    public class ResourceManagementNodes
    {
        public Entity Entity { get; set; }
        public double Availability { get; set; }
        public ResourceUsagePerTimeUnit[] ResourceUsageDetails { get; set; }
        public ResourceManagementNodes[] RelatedNodes { get; set; }
        public Entity Parent { get; set; }
        public ResourceRoleCode ResourceRole { get; set; }
        public bool IsAuthorizedForExternalUser { get; set; }
    }

    public class ResourceUsagePerTimeUnit
    {
        public DateTime Date { get; set; }
        public double Value { get; set; }
        public double Variance { get; set; }
        public bool Overloaded { get; set; }
        public bool PartlyOverloaded { get; set; }
        public bool WorkingDay { get; set; }
        public bool Uncertain { get; set; }
        public double ProjectAssignment { get; set; }
        public CapacityOverconstrainedType CapacityOverConstrained { get; set; }
    }
}
