using AirWeb.Domain.DataExchange;

namespace AirWeb.AppServices.Compliance.ComplianceMonitoring.ComplianceWorkDto.Query;

public record ComplianceEventViewDto : ComplianceWorkViewDto, IDataExchange
{
    public ushort? ActionNumber { get; init; }
    public DataExchangeStatus DataExchangeStatus { get; init; }
    public DateTimeOffset? DataExchangeStatusDate { get; init; }
}
