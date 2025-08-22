using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.EnforcementEntities.EnforcementActions.ActionProperties;
using AirWeb.Domain.Identity;
using Microsoft.Extensions.Logging;

namespace AirWeb.Domain.EnforcementEntities.EnforcementActions;

public class EnforcementActionManager(ILogger<EnforcementActionManager> logger) : IEnforcementActionManager
{
    public EnforcementAction Create(CaseFile caseFile, EnforcementActionType action, ApplicationUser? user)
    {
        var id = Guid.NewGuid();
        EnforcementAction enforcementAction = action switch
        {
            EnforcementActionType.AdministrativeOrder => new AdministrativeOrder(id, caseFile, user),
            EnforcementActionType.ConsentOrder => new ConsentOrder(id, caseFile, user),
            EnforcementActionType.InformationalLetter => new InformationalLetter(id, caseFile, user),
            EnforcementActionType.LetterOfNoncompliance => new LetterOfNoncompliance(id, caseFile, user),
            EnforcementActionType.NoFurtherActionLetter => new NoFurtherActionLetter(id, caseFile, user),
            EnforcementActionType.NoticeOfViolation => new NoticeOfViolation(id, caseFile, user),
            EnforcementActionType.NovNfaLetter => new NovNfaLetter(id, caseFile, user),
            EnforcementActionType.ProposedConsentOrder => new ProposedConsentOrder(id, caseFile, user),
            _ => throw new ArgumentOutOfRangeException(nameof(action), action, null),
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

    private static void Approve(EnforcementAction enforcementAction, ApplicationUser? user)
    {
        if (enforcementAction.IsIssued || enforcementAction.IsCanceled)
            throw new InvalidOperationException("Enforcement Action has already been issued or canceled.");
        
        enforcementAction.SetUpdater(user?.Id);
        enforcementAction.Status = EnforcementActionStatus.Approved;
        enforcementAction.ApprovedBy = user;
        enforcementAction.ApprovedDate = DateOnly.FromDateTime(DateTime.Today);
    }
    private static void ReturnToDraft(EnforcementAction enforcementAction, ApplicationUser? user)
    {
        if (enforcementAction.IsIssued || enforcementAction.IsCanceled)
            throw new InvalidOperationException("Enforcement Action has already been issued or canceled.");

        enforcementAction.SetUpdater(user?.Id);
        enforcementAction.Status = EnforcementActionStatus.Draft;
        enforcementAction.ApprovedBy = null;
        enforcementAction.ApprovedDate = null;
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

    public void RequestReview(EnforcementAction action, ApplicationUser reviewer, ApplicationUser user)
    {
        if (action.Reviews.Any(r => !r.IsCompleted))
        {
            logger.LogError("Enforcement action {Id} already has an open review request.", action.Id);
            return;
        }

        action.SetUpdater(user.Id);
        var reviewRequest = new EnforcementActionReview(Guid.NewGuid(), action, reviewer, requester: user);
        action.Reviews.Add(reviewRequest);
        action.Status = EnforcementActionStatus.ReviewRequested;
    }

    public void SubmitReview(EnforcementAction action, ReviewResult result, string? comments, ApplicationUser user,
        ApplicationUser? nextReviewer = null)
    {
        if (action.Reviews.All(r => r.IsCompleted))
        {
            logger.LogError("Enforcement action {Id} does not have an open review request.", action.Id);
            return;
        }

        action.CurrentOpenReview!.CompleteReview(user, result, comments);
        action.SetUpdater(user.Id);

        switch (result)
        {
            case ReviewResult.Approved:
                Approve(action, user);
                break;
            case ReviewResult.Returned:
                ReturnToDraft(action,user);
                break;
            case ReviewResult.Canceled:
                Cancel(action, user);
                break;
            case ReviewResult.Forwarded:
                RequestReview(action, nextReviewer!, user);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(result), result, null);
        }
    }
}
