using AirWeb.Domain.EnforcementEntities.ActionProperties;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.Identity;

namespace AirWeb.Domain.EnforcementEntities.EnforcementActions;

public interface IEnforcementActionManager
{
    public EnforcementAction Create(CaseFile caseFile, EnforcementActionType action,
        bool responseRequested, string? notes, ApplicationUser? user);

    public void AddResponse(EnforcementAction enforcementAction, DateOnly responseDate, string? comment,
        ApplicationUser? user);

    public void SetIssueDate(EnforcementAction enforcementAction, DateOnly issueDate, ApplicationUser? user);
    public void Cancel(EnforcementAction enforcementAction, ApplicationUser? user);
    public void Reopen(EnforcementAction enforcementAction, ApplicationUser? user);
    public void ExecuteOrder(ConsentOrder consentOrder, ApplicationUser? user);

    public void AddStipulatedPenalty(ConsentOrder consentOrder, StipulatedPenalty stipulatedPenalty,
        ApplicationUser? user);

    public void Delete(EnforcementAction enforcementAction, ApplicationUser? user);
}
