using AirWeb.Domain.Compliance.DataExchange;

namespace AirWeb.AppServices.Compliance.Enforcement.EnforcementActionQuery;

public record ReportableActionViewDto : ActionViewDto, IDataExchangeAction
{
    public string FacilityId { get; init; } = null!;
    public ushort? ActionNumber { get; set; }
    public DataExchangeStatus DataExchangeStatus { get; set; }
    public DateTimeOffset? DataExchangeStatusDate { get; set; }
}
