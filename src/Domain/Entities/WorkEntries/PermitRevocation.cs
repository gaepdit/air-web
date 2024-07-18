namespace AirWeb.Domain.Entities.WorkEntries;

public class PermitRevocation : BaseWorkEntry
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private PermitRevocation() { }

    internal PermitRevocation(int? id) : base(id)
    {
        WorkEntryType = WorkEntryType.PermitRevocation;
    }

    // Properties

    public DateOnly ReceivedDate { get; set; }
    public DateOnly PermitRevocationDate { get; set; }
    public DateOnly? PhysicalShutdownDate { get; set; }
    public bool FollowupTaken { get; set; }
}
