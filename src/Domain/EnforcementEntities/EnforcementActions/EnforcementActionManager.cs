using AirWeb.Domain.EnforcementEntities.ActionProperties;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.Identity;

namespace AirWeb.Domain.EnforcementEntities.EnforcementActions;

public class EnforcementActionManager : IEnforcementActionManager
{
    public EnforcementAction Create(CaseFile caseFile, EnforcementActionType action,
        bool responseRequested, string? notes, ApplicationUser? user)
    {
        EnforcementAction enforcementAction = action switch
        {
            EnforcementActionType.AdministrativeOrder => new AdministrativeOrder(Guid.NewGuid(), caseFile, user),
            EnforcementActionType.ConsentOrder => new ConsentOrder(Guid.NewGuid(), caseFile, user),
            EnforcementActionType.InformationalLetter => new InformationalLetter(Guid.NewGuid(), caseFile, user),
            EnforcementActionType.LetterOfNoncompliance => new LetterOfNoncompliance(Guid.NewGuid(), caseFile, user),
            EnforcementActionType.NoFurtherActionLetter => new NoFurtherActionLetter(Guid.NewGuid(), caseFile, user),
            EnforcementActionType.NoticeOfViolation => new NoticeOfViolation(Guid.NewGuid(), caseFile, user),
            EnforcementActionType.NovNfaLetter => new NovNfaLetter(Guid.NewGuid(), caseFile, user),
            EnforcementActionType.OrderResolvedLetter => new OrderResolvedLetter(Guid.NewGuid(), caseFile, user),
            EnforcementActionType.ProposedConsentOrder => new ProposedConsentOrder(Guid.NewGuid(), caseFile, user),
            _ => throw new ArgumentOutOfRangeException(nameof(action), action, null)
        };
        enforcementAction.Notes = notes;
        if (enforcementAction is IResponseRequested responseRequestedAction && responseRequested)
            responseRequestedAction.RequestResponse();
        caseFile.EnforcementActions.Add(enforcementAction);
        return enforcementAction;
    }

    public void SetIssueDate(EnforcementAction enforcementAction, DateOnly issueDate, ApplicationUser? user)
    {
        enforcementAction.SetUpdater(user?.Id);
        enforcementAction.IssueDate = issueDate;
        enforcementAction.Status = EnforcementActionStatus.Issued;
    }

    public void CloseAsUnsent(EnforcementAction enforcementAction, DateOnly closeDate, ApplicationUser? user)
    {
        if (enforcementAction.IsIssued)
            throw new InvalidOperationException("Enforcement Action has already been issued.");

        enforcementAction.SetUpdater(user?.Id);
        enforcementAction.ClosedAsUnsentDate = closeDate;
        enforcementAction.Status = EnforcementActionStatus.ClosedAsUnsent;
    }

    public void Reopen(EnforcementAction enforcementAction, ApplicationUser? user)
    {
        if (!enforcementAction.IsClosedAsUnsent)
            throw new InvalidOperationException("Enforcement Action is not closed.");
        enforcementAction.SetUpdater(user?.Id);
        enforcementAction.ClosedAsUnsentDate = null;

        if (enforcementAction.ApprovedDate is not null)
        {
            enforcementAction.Status = EnforcementActionStatus.Approved;
        }
        else if (enforcementAction.CurrentReviewer is not null)
        {
            enforcementAction.Status = EnforcementActionStatus.ReviewRequested;
        }
        else
        {
            enforcementAction.Status = EnforcementActionStatus.Draft;
        }
    }

    public void ExecuteOrder(ConsentOrder consentOrder, ApplicationUser? user)
    {
        throw new NotImplementedException();
        consentOrder.SetUpdater(user?.Id);
    }

    public void AddStipulatedPenalty(ConsentOrder consentOrder, StipulatedPenalty stipulatedPenalty,
        ApplicationUser? user)
    {
        throw new NotImplementedException();
        consentOrder.SetUpdater(user?.Id);
    }

    public void Delete(EnforcementAction enforcementAction, ApplicationUser? user) =>
        enforcementAction.Delete(comment: null, user);
}
