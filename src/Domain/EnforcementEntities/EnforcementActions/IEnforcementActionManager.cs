using AirWeb.Domain.EnforcementEntities.ActionProperties;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.Identity;

namespace AirWeb.Domain.EnforcementEntities.EnforcementActions;

public interface IEnforcementActionManager
{
    public EnforcementAction Create(CaseFile caseFile, EnforcementActionType action,
        ApplicationUser? user);

    // Common update methods
    public void AddResponse(EnforcementAction enforcementAction, DateOnly responseDate, string? comment,
        ApplicationUser? user);

    public void SetIssueDate(EnforcementAction enforcementAction, DateOnly? issueDate, ApplicationUser? user);
    public void Cancel(EnforcementAction enforcementAction, ApplicationUser? user);
    public void Delete(EnforcementAction enforcementAction, ApplicationUser? user);

    // Type-specific update methods
    public void Resolve(IResolvable enforcementAction, DateOnly resolvedDate, ApplicationUser? user);
    public void ExecuteOrder(IFormalEnforcementAction enforcementAction, DateOnly executedDate, ApplicationUser? user);
    public void AppealOrder(AdministrativeOrder enforcementAction, DateOnly executedDate, ApplicationUser? user);

    public StipulatedPenalty AddStipulatedPenalty(ConsentOrder consentOrder, decimal amount, DateOnly receivedDate,
        ApplicationUser? user);

    public void DeleteStipulatedPenalty(StipulatedPenalty stipulatedPenalty, ApplicationUser? user);
}
