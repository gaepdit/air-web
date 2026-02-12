using AirWeb.Core.Entities;
using AirWeb.Domain.DataExchange;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using System.Text.Json.Serialization;

namespace AirWeb.Domain.EnforcementEntities.EnforcementActions;

// All enforcement actions participating in the Data Exchange:

// * NFAs
// * DxActionEnforcementAction inheritors

public abstract class DxEnforcementAction : EnforcementAction, IDataExchange
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private protected DxEnforcementAction() { }

    private protected DxEnforcementAction(Guid id, CaseFile caseFile, ApplicationUser? user)
        : base(id, caseFile, user) { }

    [JsonIgnore]
    [StringLength(1)]
    public DataExchangeStatus DataExchangeStatus { get; set; }

    [JsonIgnore]
    public DateTimeOffset? DataExchangeStatusDate { get; set; }
}
