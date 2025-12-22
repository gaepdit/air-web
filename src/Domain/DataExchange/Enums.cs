using System.ComponentModel;

namespace AirWeb.Domain.DataExchange;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public enum DataExchangeStatus
{
    [Description("Not Included")] N,
    [Description("Processed")] P,
    [Description("Inserted")] I,
    [Description("Updated")] U,
    [Description("Deleted")] D,
}
