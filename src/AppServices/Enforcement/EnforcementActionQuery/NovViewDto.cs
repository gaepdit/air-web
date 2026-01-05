using AirWeb.Domain.DataExchange;

namespace AirWeb.AppServices.Enforcement.EnforcementActionQuery;

public record NovViewDto : ResponseRequestedViewDto, IDataExchange
{
    public string FacilityId { get; init; } = null!;
    public ushort? ActionNumber { get; init; }
    public DataExchangeStatus DataExchangeStatus { get; init; }
    public DateTimeOffset? DataExchangeStatusDate { get; init; }
}
