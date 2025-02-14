using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.Identity;

namespace AirWeb.Domain.EnforcementEntities.EnforcementActions;

public class NoFurtherActionLetter : EnforcementAction
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
