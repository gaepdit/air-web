using AirWeb.Domain.EnforcementEntities.ActionProperties;
using AirWeb.Domain.EnforcementEntities.Actions;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.Identity;

namespace AirWeb.Domain.EnforcementEntities;

public interface IEnforcementActionManager
{
    public EnforcementAction CreateEnforcementAction(CaseFile caseFile, EnforcementActionType action,
        bool responseRequested, string? notes, ApplicationUser? user);

    public void IssueEnforcementAction(EnforcementAction enforcementAction);

    public void ExecuteOrder(ConsentOrder consentOrder);

    public void AddStipulatedPenalty(ConsentOrder consentOrder, StipulatedPenalty stipulatedPenalty);
}
