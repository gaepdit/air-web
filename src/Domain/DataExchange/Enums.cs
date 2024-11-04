using System.Text.Json.Serialization;

namespace AirWeb.Domain.DataExchange;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DataExchangeStatus
{
    [UsedImplicitly] NotIncluded,
    [UsedImplicitly] Processed,
    [UsedImplicitly] Inserted,
    [UsedImplicitly] Updated,
    [UsedImplicitly] Deleted,
}
