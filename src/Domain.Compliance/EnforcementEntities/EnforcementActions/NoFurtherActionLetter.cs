using AirWeb.Domain.Compliance.EnforcementEntities.CaseFiles;
using AirWeb.Domain.Core.Entities;

namespace AirWeb.Domain.Compliance.EnforcementEntities.EnforcementActions;

public class NoFurtherActionLetter : DxEnforcementAction
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private NoFurtherActionLetter() { }

    internal NoFurtherActionLetter(Guid id, CaseFile caseFile, ApplicationUser? user)
        : base(id, caseFile, user)
    {
        ActionType = EnforcementActionType.NoFurtherActionLetter;
    }
}
