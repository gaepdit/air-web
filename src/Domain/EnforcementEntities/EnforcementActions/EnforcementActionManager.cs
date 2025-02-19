using AirWeb.Domain.EnforcementEntities.ActionProperties;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.Identity;

namespace AirWeb.Domain.EnforcementEntities.EnforcementActions;

public class EnforcementActionManager : IEnforcementActionManager
{
    public EnforcementAction CreateEnforcementAction(CaseFile caseFile, EnforcementActionType action,
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

    public void IssueEnforcementAction(EnforcementAction enforcementAction)
    {
        throw new NotImplementedException();
    }

    public void ExecuteOrder(ConsentOrder consentOrder)
    {
        throw new NotImplementedException();
    }

    public void AddStipulatedPenalty(ConsentOrder consentOrder, StipulatedPenalty stipulatedPenalty)
    {
        throw new NotImplementedException();
    }
}
