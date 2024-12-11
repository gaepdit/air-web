using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AirWeb.Domain.DataExchange;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DataExchangeStatus
{
    [Description("NotIncluded"), UsedImplicitly] N,
    [Description("Processed"), UsedImplicitly] P,
    [Description("Inserted"), UsedImplicitly] I,
    [Description("Updated"), UsedImplicitly] U,
    [Description("Deleted"), UsedImplicitly] D,
}
