using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AirWeb.AppServices.Compliance.Search;

// FUTURE: See if any of these will work with enforcement search as well.

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SortBy
{
    [Description("Id")] IdAsc,
    [Description("Id desc")] IdDesc,
    [Description("FacilityId, Id")] FacilityIdAsc,
    [Description("FacilityId desc, Id")] FacilityIdDesc,
    [Description("WorkEntryType, Id")] WorkTypeAsc,
    [Description("WorkEntryType desc, Id")] WorkTypeDesc,
    [Description("EventDate, Id")] EventDateAsc,
    [Description("EventDate desc, Id")] EventDateDesc,
    [Description("Year, Id")] YearAsc,
    [Description("Year desc, Id")] YearDesc,
}

// "(Any)" (null) = no filtering
// "Yes" = only if value is true
// "No" = only if value is false
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum YesNoAny
{
    Yes = 1,
    No = 0,
}

// "(Any)" (null) = no filtering
// "Closed" = only if "closed" value is true
// "Open" = only if "closed" value is false
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ClosedOpenAny
{
    Closed = 1,
    Open = 0,
}

// "Not Deleted" (null) = only non-deleted entries
// "Deleted" = only deleted entries
// "All" = both deleted and not-deleted entries
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DeleteStatus
{
    Deleted = 0,
    All = 1,
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum WorkTypeSearch
{
    [Description("Annual Compliance Certifications")] Acc,
    [Description("Inspections")] Inspection,
    [Description("RMP Inspections")] Rmp,
    [Description("Reports")] Report,
    [Description("Source Test Compliance Reviews")] Str,
    [Description("Notifications")] Notification,
    [Description("Permit Revocations")] PermitRevocation,
}
