using AirWeb.Domain.Compliance.DataExchange;

namespace AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.ComplianceWorkDto.Query;

public record ComplianceEventViewDto : ComplianceWorkViewDto, IDataExchangeAction
{
    public ushort? ActionNumber { get; set; }
    public DataExchangeStatus DataExchangeStatus { get; set; }
    public DateTimeOffset? DataExchangeStatusDate { get; set; }
}
