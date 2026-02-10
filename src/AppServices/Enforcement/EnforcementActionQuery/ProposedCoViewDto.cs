using AirWeb.Domain.DataExchange;

namespace AirWeb.AppServices.Enforcement.EnforcementActionQuery;

public record ProposedCoViewDto : ResponseRequestedViewDto, IDataExchangeAction
{
    public string FacilityId { get; init; } = null!;
    public ushort? ActionNumber { get; set; }
    public DataExchangeStatus DataExchangeStatus { get; set; }
    public DateTimeOffset? DataExchangeStatusDate { get; set; }
}
