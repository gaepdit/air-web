using AirWeb.Domain.BaseEntities.Interfaces;

namespace AirWeb.Domain.DataExchange;

// EPA Data Exchange properties are used by compliance monitoring, case files and enforcement actions.
public interface IDataExchange : IFacilityId
{
    public ushort? ActionNumber { get; }
    public DataExchangeStatus DataExchangeStatus { get; }
    public DateTimeOffset? DataExchangeStatusDate { get; }

    // EPA Compliance ID
    // SQL version: 
    // CONCAT('GA000A0000', SUBSTRING(STRAIRSNUMBER, 3, 10), dbo.LPAD(STRAFSACTIONNUMBER, 5, '0'))
    public string? EpaActionId =>
        ActionNumber is null ? null : $"GA000A000013{((FacilityId)FacilityId).Id}{ActionNumber:D5}";

    public string EpaFacilityIdentifier => ((FacilityId)FacilityId).EpaFacilityIdentifier;
}
