using AirWeb.Domain.AuditPoints;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.EnforcementEntities.EnforcementActions.ActionProperties;
using AirWeb.Domain.Identity;
using Microsoft.Extensions.Logging;
using ZLogger;

namespace AirWeb.Domain.EnforcementEntities.EnforcementActions;

public class EnforcementActionManager(
    ICaseFileManager caseFileManager,
    IFacilityService facilityService,
    ILogger<EnforcementActionManager> logger) : IEnforcementActionManager
{
    public async Task<EnforcementAction> CreateAsync(CaseFile caseFile, EnforcementActionType actionType,
        ApplicationUser? user)
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

        if (enforcementAction is ReportableEnforcementAction reportableEnforcementAction)
        {
            var actionNumber = await facilityService.GetNextActionNumberAsync((FacilityId)caseFile.FacilityId)
                .ConfigureAwait(false);
            reportableEnforcementAction.ActionNumber = actionNumber;
        }

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

    public void SetIssuedStatus(EnforcementAction action, DateOnly? issueDate, ApplicationUser? user)
    {
        if (action.IsCanceled)
            throw new InvalidOperationException("Enforcement Action has been canceled.");

        action.SetUpdater(user?.Id);
        action.Status = issueDate.HasValue ? EnforcementActionStatus.Issued : EnforcementActionStatus.Draft;
    }

    public bool Issue(EnforcementAction action, DateOnly issueDate, ApplicationUser? user,
        bool tryCloseCaseFile = false)
    {
        if (action.IsCanceled)
            throw new InvalidOperationException("Enforcement Action has been canceled.");

        action.SetUpdater(user?.Id);
        action.IssueDate = issueDate;
        action.Status = EnforcementActionStatus.Issued;

        if (!tryCloseCaseFile || action is not
            {
                ActionType: EnforcementActionType.NovNfaLetter or EnforcementActionType.NoFurtherActionLetter,
                CaseFile.MissingData: false,
            })
            return false;

        var caseFile = action.CaseFile;
        caseFileManager.Close(caseFile, user);
        return true;
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
    public bool Resolve(EnforcementAction action, DateOnly resolvedDate, ApplicationUser? user,
        bool tryCloseCaseFile = false)
    {
        if (action is not IResolvable resolvable || resolvable.IsResolved)
            throw new InvalidOperationException("Enforcement Action is not resolvable.");

        action.SetUpdater(user?.Id);
        resolvable.Resolve(resolvedDate);

        if (!tryCloseCaseFile) return false;

        var caseFile = action.CaseFile;
        caseFileManager.Close(caseFile, user);
        return true;
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
            logger.ZLogError($"Enforcement action {action.Id} already has an open review request.");
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
            logger.ZLogError($"Enforcement action {action.Id} does not have an open review request.");
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
