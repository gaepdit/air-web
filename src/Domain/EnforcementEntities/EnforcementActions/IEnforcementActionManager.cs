using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.EnforcementEntities.EnforcementActions.ActionProperties;
using AirWeb.Domain.Identity;

namespace AirWeb.Domain.EnforcementEntities.EnforcementActions;

public interface IEnforcementActionManager
{
    EnforcementAction Create(CaseFile caseFile, EnforcementActionType action, ApplicationUser? user);

    // Common update methods
    void AddResponse(EnforcementAction enforcementAction, DateOnly responseDate, string? comment,
        ApplicationUser? user);

    void SetIssueDate(EnforcementAction enforcementAction, DateOnly? issueDate, ApplicationUser? user);
    void Cancel(EnforcementAction enforcementAction, ApplicationUser? user);
    void Delete(EnforcementAction enforcementAction, ApplicationUser? user);

    // Type-specific update methods
    void Resolve(IResolvable enforcementAction, DateOnly resolvedDate, ApplicationUser? user);
    void ExecuteOrder(IFormalEnforcementAction enforcementAction, DateOnly executedDate, ApplicationUser? user);
    void AppealOrder(AdministrativeOrder enforcementAction, DateOnly executedDate, ApplicationUser? user);

    // Stipulated penalties
    StipulatedPenalty AddStipulatedPenalty(ConsentOrder consentOrder, decimal amount, DateOnly receivedDate,
        ApplicationUser? user);

    void DeleteStipulatedPenalty(StipulatedPenalty stipulatedPenalty, ApplicationUser? user);

    // Reviews
    void RequestReview(EnforcementAction enforcementAction, ApplicationUser reviewer, ApplicationUser user);

    void SubmitReview(EnforcementAction enforcementAction, ReviewResult result, string? comments, ApplicationUser user,
        ApplicationUser? nextReviewer = null);
}
