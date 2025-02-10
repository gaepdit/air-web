using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.Identity;

namespace AirWeb.Domain.EnforcementEntities.Actions;

public class ProposedConsentOrder : EnforcementAction, IInformalEnforcementAction, IResponseRequested
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private ProposedConsentOrder() { }

    internal ProposedConsentOrder(Guid id, NoticeOfViolation noticeOfViolation, ApplicationUser? user)
        : base(id, noticeOfViolation.CaseFile, user)
    {
        ActionType = EnforcementActionType.ProposedConsentOrder;
        ActionsToBeAddressed.Add(noticeOfViolation);
    }

    internal ProposedConsentOrder(Guid id, CaseFile caseFile, ApplicationUser? user)
        : base(id, caseFile, user)
    {
        ActionType = EnforcementActionType.ProposedConsentOrder;
    }

    public ICollection<IInformalEnforcementAction> ActionsToBeAddressed { get; init; } = [];
    public bool ResponseRequested => true;
    public DateOnly? ResponseReceived { get; set; }

    [StringLength(7000)]
    public string? ResponseComment { get; set; }

    public IFormalEnforcementAction? Order { get; set; }
    public NoFurtherActionLetter? NoFurtherActionLetter { get; set; }
}
