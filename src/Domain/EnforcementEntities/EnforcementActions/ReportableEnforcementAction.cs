using AirWeb.Domain.DataExchange;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.Identity;
using System.Text.Json.Serialization;

namespace AirWeb.Domain.EnforcementEntities.EnforcementActions;

// All enforcement reportable to the Data Exchange:

// Informal Enforcement Actions:
// * Notices of Violation
// * Combined NOV/NFAs
// * Proposed Consent Orders

// Formal Enforcement Actions:
// * Consent Orders
// * Administrative Orders

public abstract class ReportableEnforcementAction : EnforcementAction, IDataExchange, IDataExchangeWrite
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private protected ReportableEnforcementAction() { }

    private protected ReportableEnforcementAction(Guid id, CaseFile caseFile, ApplicationUser? user)
        : base(id, caseFile, user) { }

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
        if (ActionNumber is null) return;
        DataExchangeStatus = status;
        DataExchangeStatusDate = DateTimeOffset.Now;
    }
}
