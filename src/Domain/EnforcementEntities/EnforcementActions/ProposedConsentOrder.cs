using AirWeb.Core.Entities;
using AirWeb.Domain.EnforcementEntities.CaseFiles;

namespace AirWeb.Domain.EnforcementEntities.EnforcementActions;

public class ProposedConsentOrder : DxActionEnforcementAction, IInformalEnforcementAction, IResponseRequested
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private ProposedConsentOrder() { }

    internal ProposedConsentOrder(Guid id, CaseFile caseFile, ApplicationUser? user)
        : base(id, caseFile, user)
    {
        ActionType = EnforcementActionType.ProposedConsentOrder;
    }

    public bool ResponseRequested { get; set; }
    public DateOnly? ResponseReceived { get; set; }

    [StringLength(7000)]
    public string? ResponseComment { get; set; }
}
