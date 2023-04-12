namespace Ekin.Clarizen.Common
{
    public enum ResourceLoadFilterType
    {
        None,
        Billable,
        NotBillable
    }

    public enum UserSortingType
    {
        InTwoGroups,
        ByList,
        Alfabet,
        NoSort
    }

    public enum ResourceLoadValueType
    {
        Load,
        Availability
    }

    public enum ResourceLoadBaseType
    {
        Work,
        RemainingEffort
    }

    public enum ResourceLoadUnitType
    {
        Units,
        Hours,
        FTE,
        PersonDays
    }

    public enum ReportingPeriodType
    {
        Daily,
        Weekly,
        Monthly,
        Quarterly,
        Yearly
    }

    public enum ResourceLoadRollupType
    {
        Load,
        ProjectAssignment
    }

    public enum CapacityOverconstrainedType
    {
        None,
        Partial,
        Full
    }

    public enum ResourceRoleCode
    {
        Regular,
        Manager,
        AdditionalManager
    }
}
