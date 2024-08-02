using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AirWeb.AppServices.Compliance.Search;

// FUTURE: See if these will work with enforcement as well.

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SortBy
{
    [Description("Id")] IdAsc,
    [Description("Id desc")] IdDesc,
    [Description("ReceivedDate, Id")] ReceivedDateAsc,
    [Description("ReceivedDate desc, Id")] ReceivedDateDesc,
    [Description("Status, Id")] StatusAsc,
    [Description("Status desc, Id")] StatusDesc,
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OpenStatus
{
    [Description("Only Open")] Open = 0,
    [Description("Only Closed")] Closed = 1,
    [Description("All")] All = 2,
}

// "Not Deleted" is included as an additional Delete Status option in the UI representing the null default state.
// "Deleted" = only deleted entries
// "All" = both deleted and not-deleted entries
// "Not Deleted" (null) = only non-deleted entries
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DeleteStatus
{
    Deleted = 0,
    All = 1,
}
