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

    public void AddResponse(EnforcementAction enforcementAction, DateOnly responseDate, string? comment,
        ApplicationUser? user)
    {
        if (enforcementAction is not IResponseRequested responseRequested) throw new InvalidOperationException();
        enforcementAction.SetUpdater(user?.Id);
        responseRequested.ResponseReceived = responseDate;
        responseRequested.ResponseComment = comment;
    }

    public void SetIssueDate(EnforcementAction enforcementAction, DateOnly issueDate, ApplicationUser? user)
    {
        if (enforcementAction.IsCanceled)
            throw new InvalidOperationException("Enforcement Action has been canceled.");

        enforcementAction.SetUpdater(user?.Id);
        enforcementAction.IssueDate = issueDate;
        enforcementAction.Status = EnforcementActionStatus.Issued;
    }

    public void Cancel(EnforcementAction enforcementAction, ApplicationUser? user)
    {
        if (enforcementAction.IsIssued)
            throw new InvalidOperationException("Enforcement Action has already been issued.");

        enforcementAction.SetUpdater(user?.Id);
        enforcementAction.CanceledDate = DateOnly.FromDateTime(DateTime.Today);
        enforcementAction.Status = EnforcementActionStatus.Canceled;
    }

    public void Reopen(EnforcementAction enforcementAction, ApplicationUser? user)
    {
        if (!enforcementAction.IsCanceled)
            throw new InvalidOperationException("Enforcement Action has not been canceled.");

        enforcementAction.SetUpdater(user?.Id);
        enforcementAction.CanceledDate = null;

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

    public void Resolve(EnforcementAction enforcementAction, DateOnly resolvedDate, ApplicationUser? user)
    {
        if (enforcementAction is not IResolvable resolvableAction)
            throw new InvalidOperationException("Enforcement action is not resolvable");

        if (resolvableAction.IsResolved)
            throw new InvalidOperationException("Enforcement Action has already been resolved.");

        enforcementAction.SetUpdater(user?.Id);
        resolvableAction.Resolve(resolvedDate);
    }

    public void Delete(EnforcementAction enforcementAction, ApplicationUser? user) =>
        enforcementAction.Delete(comment: null, user);
}
