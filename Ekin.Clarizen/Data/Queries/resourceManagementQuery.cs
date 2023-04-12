using Newtonsoft.Json;
using System;

namespace Ekin.Clarizen.Data.Queries
{
    public class ResourceManagementQuery
    {
        public ResourceManagementQuery()
        {
            Definition = new ResourceManagementQueryDefinition();
        }
        [JsonProperty("resourceEntitiesIds")]
        public string[] ResourceEntitiesIds { get; set; }
        [JsonProperty("definition")]
        public ResourceManagementQueryDefinition Definition { get; set; }
    }

    public class ResourceManagementQueryDefinition
    {
        [JsonProperty("setting")]
        public ResourceManagementQuerySetting Setting { get; set; }
        [JsonProperty("globalFilters")]
        public ResourceManagementQueryGlobalFilters GlobalFilters { get; set; }
        [JsonProperty("presentationFilters")]
        public ResourceManagementQueryPresentationFilter PresentationFilters { get; set; }
    }

    public class ResourceManagementQuerySetting
    {
        [JsonProperty("fields")]
        public FiledsToBeSelected[] Fields { get; set; }
        [JsonProperty("sortingType")]
        public string SortingType { get; set; }
        [JsonProperty("workOrRemainingEffort")]
        public string WorkOrRemainingEffort { get; set; }
        [JsonProperty("loadOrAvailability")]
        public string LoadOrAvailability { get; set; }
        [JsonProperty("hoursView")]
        public bool HoursView { get; set; }
        [JsonProperty("unitType")]
        public string UnitType { get; set; }
        [JsonProperty("reportingPeriod")]
        public string ReportingPeriod { get; set; }
        [JsonProperty("includeProjectAssignment")]
        public bool IncludeProjectAssignment { get; set; }
        [JsonProperty("loadBasedOnProjectAssignment")]
        public bool LoadBasedOnProjectAssignment { get; set; }
        [JsonProperty("rollupType")]
        public string RollupType { get; set; }
    }

    public class FiledsToBeSelected
    {
        [JsonProperty("typeName")]
        public string TypeName { get; set; }
        [JsonProperty("fieldNames")]
        public string[] FieldNames { get; set; }
    }

    public class ResourceManagementQueryGlobalFilters
    {
        [JsonProperty("timeFilter")]
        public ResourceManagementQueryTimefilter TimeFilter { get; set; }
        [JsonProperty("wantedStates")]
        public string[] WantedStates { get; set; }
        [JsonProperty("resourceLoadFilterType")]
        public string ResourceLoadFilterType { get; set; }
        [JsonProperty("projectTypes")]
        public ResourceManagementQueryProjectTypes[] ProjectTypes { get; set; }
        [JsonProperty("projectsList")]
        public EntityId[] ProjectsList { get; set; }
    }

    public class ResourceManagementQueryTimefilter
    {
        [JsonProperty("from")]
        public DateTime From { get; set; }
        [JsonProperty("to")]
        public DateTime To { get; set; }
    }

    public class ResourceManagementQueryPresentationFilter
    {
        [JsonProperty("containsInName")]
        public string ContainsInName { get; set; }
    }

    public class ResourceManagementQueryProjectTypes
    {
        [JsonProperty("value")]
        public string Value { get; set; }
        [JsonProperty("displayValue")]
        public string DisplayValue { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("internalId")]
        public string InternalId { get; set; }
        [JsonProperty("color")]
        public string Color { get; set; }
        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }
    }
}
