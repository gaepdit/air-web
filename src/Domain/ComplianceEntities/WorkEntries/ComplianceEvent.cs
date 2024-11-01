using AirWeb.Domain.DataExchange;
using AirWeb.Domain.EnforcementEntities.Cases;
using System.Text.Json.Serialization;

namespace AirWeb.Domain.ComplianceEntities.WorkEntries;

public abstract class ComplianceEvent : WorkEntry
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private protected ComplianceEvent() { }

    private protected ComplianceEvent(int? id) : base(id) { }

    public ICollection<EnforcementCase> EnforcementCases { get; set; } = [];

    // Data exchange properties
    public bool IsDataFlowEnabled => IsClosed && !IsDeleted && WorkEntryType != WorkEntryType.RmpInspection;

    [JsonIgnore]
    [StringLength(11)]
    public DataExchangeStatus DataExchangeStatus { get; init; }
}
