using AirWeb.Domain.DataExchange;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.Identity;
using System.Text.Json.Serialization;

namespace AirWeb.Domain.ComplianceEntities.WorkEntries;

public abstract class ComplianceEvent : WorkEntry, IDataExchange, IDataExchangeWrite
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private protected ComplianceEvent() { }

    private protected ComplianceEvent(int? id, FacilityId facilityId, ApplicationUser? user)
        : base(id, facilityId, user)
    {
        IsComplianceEvent = true;
    }

    public ICollection<CaseFile> CaseFiles { get; } = [];

    // Data exchange properties

    [JsonIgnore]
    public ushort? ActionNumber { get; internal set; }

    [JsonIgnore]
    [StringLength(1)]
    public DataExchangeStatus DataExchangeStatus { get; internal set; }

    [JsonIgnore]
    public DateTimeOffset? DataExchangeStatusDate { get; internal set; }

    void IDataExchangeWrite.SetActionNumber(ushort actionNumber)
    {
        ActionNumber = actionNumber;
        DataExchangeStatus = DataExchangeStatus.I;
        DataExchangeStatusDate = DateTimeOffset.Now;
    }

    void IDataExchangeWrite.SetDataExchangeStatus(DataExchangeStatus status)
    {
        DataExchangeStatus = status;
        DataExchangeStatusDate = DateTimeOffset.Now;
    }
}
