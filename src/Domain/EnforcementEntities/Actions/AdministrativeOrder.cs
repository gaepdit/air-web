using AirWeb.Domain.EnforcementEntities.Cases;
using AirWeb.Domain.Identity;

namespace AirWeb.Domain.EnforcementEntities.Actions;

public class AdministrativeOrder : EnforcementAction, IExecutable
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
    public AoResolvedLetter? ResolvedLetter { get; set; }
}
