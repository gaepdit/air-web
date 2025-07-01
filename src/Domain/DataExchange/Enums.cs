using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AirWeb.Domain.DataExchange;

[JsonConverter(typeof(JsonStringEnumConverter))]
[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public enum DataExchangeStatus
{
    [Description("NotIncluded")] N,
    [Description("Processed")] P,
    [Description("Inserted")] I,
    [Description("Updated")] U,
    [Description("Deleted")] D,
}
