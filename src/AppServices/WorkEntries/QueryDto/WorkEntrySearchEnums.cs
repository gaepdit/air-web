using System.ComponentModel;
using System.Text.Json.Serialization;

namespace MyApp.AppServices.WorkEntries.QueryDto;

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


// "Not Deleted" is included as an additional Delete Status option in the UI representing the null default state.
// "Deleted" = only deleted entries
// "All" = all entries
// "Not Deleted" (null) = only non-deleted entries
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SearchDeleteStatus
{
    Deleted = 0,
    All = 1,
}
