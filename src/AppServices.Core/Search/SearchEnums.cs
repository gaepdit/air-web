using System.Text.Json.Serialization;

namespace AirWeb.AppServices.Core.Search;

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

// "Not Deleted" (null) = only non-deleted records
// "Deleted" = only deleted records
// "All" = both deleted and not-deleted records
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DeleteStatus
{
    Deleted = 0,
    All = 1,
}
