using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.Identity;

namespace AirWeb.Domain.EnforcementEntities.Actions;

public class NoticeOfViolation : EnforcementAction, IInformalEnforcementAction, IResponseRequested
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private NoticeOfViolation() { }

    internal NoticeOfViolation(Guid id, CaseFile caseFile, ApplicationUser? user)
        : base(id, caseFile, user)
    {
        ActionType = EnforcementActionType.NoticeOfViolation;
    }

    public bool ResponseRequested => true;
    public DateOnly? ResponseReceived { get; set; }

    [StringLength(7000)]
    public string? ResponseComment { get; set; }

    public ICollection<ProposedConsentOrder> ProposedConsentOrders { get; } = [];
    public IFormalEnforcementAction? Order { get; set; }
    public NoFurtherActionLetter? NoFurtherActionLetter { get; set; }
}
