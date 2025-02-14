using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.Identity;

namespace AirWeb.Domain.EnforcementEntities.EnforcementActions;

public class AdministrativeOrder : EnforcementAction, IFormalEnforcementAction
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private AdministrativeOrder() { }

    internal AdministrativeOrder(Guid id, CaseFile caseFile, ApplicationUser? user)
        : base(id, caseFile, user)
    {
        ActionType = EnforcementActionType.AdministrativeOrder;
    }

    public DateOnly? ExecutedDate { get; set; }
    public bool IsExecuted => ExecutedDate.HasValue;
    public DateOnly? AppealedDate { get; set; }
    public DateOnly? ResolvedDate { get; set; }
    public bool IsResolved => ResolvedDate.HasValue;
}
