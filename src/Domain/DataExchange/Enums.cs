using System.ComponentModel;

namespace AirWeb.Domain.DataExchange;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public enum DataExchangeStatus
{
    [Description("NotIncluded")] N,
    [Description("Processed")] P,
    [Description("Inserted")] I,
    [Description("Updated")] U,
    [Description("Deleted")] D,
}
