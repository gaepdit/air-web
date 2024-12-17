using AirWeb.Domain.EnforcementEntities.Cases;
using AirWeb.Domain.Identity;

namespace AirWeb.Domain.EnforcementEntities.Actions;

public class AdministrativeOrder : EnforcementAction, IResolvable
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private AdministrativeOrder() { }

    internal AdministrativeOrder(Guid id, CaseFile caseFile, ApplicationUser? user) :
        base(id, caseFile, user)
    {
        EnforcementActionType = EnforcementActionType.AdministrativeOrder;
    }

    public DateOnly? ExecutedDate { get; set; }
    public DateOnly? AppealedDate { get; set; }
    public DateOnly? ResolvedDate { get; set; }
    public bool IsResolved => ResolvedDate.HasValue;
    public AoResolvedLetter? ResolvedLetter { get; set; }
}
