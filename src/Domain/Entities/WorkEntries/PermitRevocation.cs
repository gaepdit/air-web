namespace AirWeb.Domain.Entities.WorkEntries;

public class PermitRevocation : BaseWorkEntry
{
    internal PermitRevocation(int? id) : base(id) => WorkEntryType = WorkEntryType.PermitRevocation;

    public DateOnly ReceivedDate { get; init; }
    public DateOnly PermitRevocationDate { get; init; }
    public DateOnly? PhysicalShutdownDate { get; init; }
    public bool FollowupTaken { get; init; }
}
