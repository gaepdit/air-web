using AirWeb.Domain.EnforcementEntities.Cases;

namespace AirWeb.Domain.EnforcementEntities.Actions;

public class EnforcementAction : AuditableSoftDeleteEntity<Guid>
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private EnforcementAction() { }

    internal EnforcementAction(Guid id, EnforcementCase enforcementCase)
    {
        Id = id;
        EnforcementCase = enforcementCase;
    }

    public EnforcementCase EnforcementCase { get; set; } = null!;
}
