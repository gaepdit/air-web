using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.EnforcementEntities.EnforcementActions.ActionProperties;
using AirWeb.Domain.Identity;

namespace AirWeb.Domain.EnforcementEntities.EnforcementActions;

public interface IEnforcementActionManager
{
    EnforcementAction Create(CaseFile caseFile, EnforcementActionType actionType, ApplicationUser? user);

    // Common update methods
    void AddResponse(EnforcementAction action, DateOnly responseDate, string? comment, ApplicationUser? user);
    void SetIssueDate(EnforcementAction action, DateOnly? issueDate, ApplicationUser? user);
    void Cancel(EnforcementAction action, ApplicationUser? user);
    void Delete(EnforcementAction action, CaseFile caseFile, ApplicationUser? user);

    // Type-specific update methods
    void Resolve(IResolvable action, DateOnly resolvedDate, ApplicationUser? user);
    void ExecuteOrder(IFormalEnforcementAction action, DateOnly executedDate, ApplicationUser? user);
    void AppealOrder(AdministrativeOrder action, DateOnly executedDate, ApplicationUser? user);

    // Stipulated penalties
    StipulatedPenalty AddStipulatedPenalty(ConsentOrder consentOrder, decimal amount, DateOnly receivedDate,
        ApplicationUser? user);

    void DeleteStipulatedPenalty(StipulatedPenalty stipulatedPenalty, ApplicationUser? user);

    // Reviews
    void RequestReview(EnforcementAction action, ApplicationUser reviewer, ApplicationUser user);

    void SubmitReview(EnforcementAction action, ReviewResult result, string? comments, ApplicationUser user,
        ApplicationUser? nextReviewer = null);
}
