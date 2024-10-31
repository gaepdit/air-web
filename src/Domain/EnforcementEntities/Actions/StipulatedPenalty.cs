namespace AirWeb.Domain.EnforcementEntities.Actions;

public class StipulatedPenalty : AuditableSoftDeleteEntity<Guid>
{
    public EnforcementAction EnforcementAction { get; set; } = null!;
}
