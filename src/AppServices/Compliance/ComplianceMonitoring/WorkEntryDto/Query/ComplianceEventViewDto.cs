using AirWeb.Domain.DataExchange;

namespace AirWeb.AppServices.Compliance.ComplianceMonitoring.WorkEntryDto.Query;

public record ComplianceEventViewDto : WorkEntryViewDto, IDataExchange
{
    public ushort? ActionNumber { get; init; }
    public DataExchangeStatus DataExchangeStatus { get; init; }
    public DateTimeOffset? DataExchangeStatusDate { get; init; }
}
