using AirWeb.Core.Entities;
using AirWeb.Domain.EnforcementEntities.CaseFiles;

namespace AirWeb.Domain.EnforcementEntities.EnforcementActions;

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
