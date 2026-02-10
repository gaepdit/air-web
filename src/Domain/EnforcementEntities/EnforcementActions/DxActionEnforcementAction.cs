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

public abstract class DxActionEnforcementAction : DxEnforcementAction, IDataExchangeAction
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private protected DxActionEnforcementAction() { }

    private protected DxActionEnforcementAction(Guid id, CaseFile caseFile, ApplicationUser? user)
        : base(id, caseFile, user)
    {
        IsReportableAction = true;
    }

    // Data exchange properties
    [JsonIgnore]
    public ushort? ActionNumber { get; set; }
}
