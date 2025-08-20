using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.EnforcementEntities.EnforcementActions.ActionProperties;
using AirWeb.Domain.Identity;

namespace AirWeb.Domain.EnforcementEntities.EnforcementActions;

public class EnforcementActionManager : IEnforcementActionManager
{
    public EnforcementAction Create(CaseFile caseFile, EnforcementActionType action, ApplicationUser? user)
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
            EnforcementActionType.ProposedConsentOrder => new ProposedConsentOrder(Guid.NewGuid(), caseFile, user),
            _ => throw new ArgumentOutOfRangeException(nameof(action), action, null)
        };

        caseFile.EnforcementActions.Add(enforcementAction);
        return enforcementAction;
    }

    // Common update methods
    public void AddResponse(EnforcementAction enforcementAction, DateOnly responseDate, string? comment,
        ApplicationUser? user)
    {
        if (enforcementAction is not IResponseRequested responseRequested) throw new InvalidOperationException();
        enforcementAction.SetUpdater(user?.Id);
        responseRequested.ResponseReceived = responseDate;
        responseRequested.ResponseComment = comment;
    }

    public void SetIssueDate(EnforcementAction enforcementAction, DateOnly? issueDate, ApplicationUser? user)
    {
        if (enforcementAction.IsCanceled)
            throw new InvalidOperationException("Enforcement Action has been canceled.");

        enforcementAction.SetUpdater(user?.Id);
        enforcementAction.IssueDate = issueDate;
        enforcementAction.Status = issueDate.HasValue ? EnforcementActionStatus.Issued : EnforcementActionStatus.Draft;
    }

    public void Cancel(EnforcementAction enforcementAction, ApplicationUser? user)
    {
        if (enforcementAction.IsIssued)
            throw new InvalidOperationException("Enforcement Action has already been issued.");

        enforcementAction.SetUpdater(user?.Id);
        enforcementAction.CanceledDate = DateOnly.FromDateTime(DateTime.Today);
        enforcementAction.Status = EnforcementActionStatus.Canceled;
    }

    public void Delete(EnforcementAction enforcementAction, ApplicationUser? user) =>
        enforcementAction.Delete(comment: null, user);

    // Type-specific update methods
    public void Resolve(IResolvable enforcementAction, DateOnly resolvedDate, ApplicationUser? user)
    {
        if (enforcementAction.IsResolved)
            throw new InvalidOperationException("Enforcement Action has already been resolved.");

        ((EnforcementAction)enforcementAction).SetUpdater(user?.Id);
        enforcementAction.Resolve(resolvedDate);
    }

    public void ExecuteOrder(IFormalEnforcementAction enforcementAction, DateOnly executedDate, ApplicationUser? user)
    {
        ((EnforcementAction)enforcementAction).SetUpdater(user?.Id);
        enforcementAction.Execute(executedDate);
    }

    public void AppealOrder(AdministrativeOrder enforcementAction, DateOnly executedDate, ApplicationUser? user)
    {
        enforcementAction.SetUpdater(user?.Id);
        enforcementAction.Appeal(executedDate);
    }

    public StipulatedPenalty AddStipulatedPenalty(ConsentOrder consentOrder, decimal amount, DateOnly receivedDate,
        ApplicationUser? user)
    {
        var penalty = new StipulatedPenalty(Guid.NewGuid(), consentOrder, amount, receivedDate, user);
        consentOrder.StipulatedPenalties.Add(penalty);
        return penalty;
    }

    public void DeleteStipulatedPenalty(StipulatedPenalty stipulatedPenalty, ApplicationUser? user) =>
        stipulatedPenalty.SetDeleted(user?.Id);
}
