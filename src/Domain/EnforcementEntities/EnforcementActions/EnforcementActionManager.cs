using AirWeb.Domain.AuditPoints;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.EnforcementEntities.EnforcementActions.ActionProperties;
using AirWeb.Domain.Identity;
using Microsoft.Extensions.Logging;

namespace AirWeb.Domain.EnforcementEntities.EnforcementActions;

public class EnforcementActionManager(ILogger<EnforcementActionManager> logger) : IEnforcementActionManager
{
    public EnforcementAction Create(CaseFile caseFile, EnforcementActionType actionType, ApplicationUser? user)
    {
        var id = Guid.NewGuid();
        EnforcementAction enforcementAction = actionType switch
        {
            EnforcementActionType.AdministrativeOrder => new AdministrativeOrder(id, caseFile, user),
            EnforcementActionType.ConsentOrder => new ConsentOrder(id, caseFile, user),
            EnforcementActionType.InformationalLetter => new InformationalLetter(id, caseFile, user),
            EnforcementActionType.LetterOfNoncompliance => new LetterOfNoncompliance(id, caseFile, user),
            EnforcementActionType.NoFurtherActionLetter => new NoFurtherActionLetter(id, caseFile, user),
            EnforcementActionType.NoticeOfViolation => new NoticeOfViolation(id, caseFile, user),
            EnforcementActionType.NovNfaLetter => new NovNfaLetter(id, caseFile, user),
            EnforcementActionType.ProposedConsentOrder => new ProposedConsentOrder(id, caseFile, user),
            _ => throw new ArgumentOutOfRangeException(nameof(actionType), actionType, null),
        };

        caseFile.EnforcementActions.Add(enforcementAction);
        caseFile.AuditPoints.Add(CaseFileAuditPoint.EnforcementActionAdded(actionType, user));
        return enforcementAction;
    }

    // Common update methods
    public void AddResponse(EnforcementAction action, DateOnly responseDate, string? comment, ApplicationUser? user)
    {
        if (action is not IResponseRequested responseRequested) throw new InvalidOperationException();
        action.SetUpdater(user?.Id);
        responseRequested.ResponseReceived = responseDate;
        responseRequested.ResponseComment = comment;
    }

    public void SetIssueDate(EnforcementAction action, DateOnly? issueDate, ApplicationUser? user)
    {
        if (action.IsCanceled)
            throw new InvalidOperationException("Enforcement Action has been canceled.");

        action.SetUpdater(user?.Id);
        action.IssueDate = issueDate;
        action.Status = issueDate.HasValue ? EnforcementActionStatus.Issued : EnforcementActionStatus.Draft;
    }

    private static void Approve(EnforcementAction action, ApplicationUser? user)
    {
        if (action.IsIssued || action.IsCanceled)
            throw new InvalidOperationException("Enforcement Action has already been issued or canceled.");

        action.SetUpdater(user?.Id);
        action.Status = EnforcementActionStatus.Approved;
        action.ApprovedBy = user;
        action.ApprovedDate = DateTime.Now;
    }

    private static void ReturnToDraft(EnforcementAction action, ApplicationUser? user)
    {
        if (action.IsIssued || action.IsCanceled)
            throw new InvalidOperationException("Enforcement Action has already been issued or canceled.");

        action.SetUpdater(user?.Id);
        action.Status = EnforcementActionStatus.Draft;
        action.ApprovedBy = null;
        action.ApprovedDate = null;
    }

    public void Cancel(EnforcementAction action, ApplicationUser? user)
    {
        if (action.IsIssued)
            throw new InvalidOperationException("Enforcement Action has already been issued.");

        action.SetUpdater(user?.Id);
        action.CanceledDate = DateTime.Now;
        action.Status = EnforcementActionStatus.Canceled;
    }

    public void Delete(EnforcementAction action, CaseFile caseFile, ApplicationUser? user)
    {
        action.Delete(comment: null, user);
        caseFile.AuditPoints.Add(CaseFileAuditPoint.EnforcementActionDeleted(action.ActionType, user));
    }

    // Type-specific update methods
    public void Resolve(IResolvable action, DateOnly resolvedDate, ApplicationUser? user)
    {
        if (action.IsResolved)
            throw new InvalidOperationException("Enforcement Action has already been resolved.");

        ((EnforcementAction)action).SetUpdater(user?.Id);
        action.Resolve(resolvedDate);
    }

    public void ExecuteOrder(IFormalEnforcementAction action, DateOnly executedDate, ApplicationUser? user)
    {
        ((EnforcementAction)action).SetUpdater(user?.Id);
        action.Execute(executedDate);
    }

    public void AppealOrder(AdministrativeOrder action, DateOnly executedDate, ApplicationUser? user)
    {
        action.SetUpdater(user?.Id);
        action.Appeal(executedDate);
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
                ReturnToDraft(action, user);
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
