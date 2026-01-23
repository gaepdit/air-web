using AirWeb.Domain.DataExchange;

namespace AirWeb.AppServices.Compliance.ComplianceMonitoring.ComplianceWorkDto.Query;

public record ComplianceEventViewDto : ComplianceWorkViewDto, IDataExchange
{
    public ushort? ActionNumber { get; set; }
    public DataExchangeStatus DataExchangeStatus { get; set; }
    public DateTimeOffset? DataExchangeStatusDate { get; set; }
}
