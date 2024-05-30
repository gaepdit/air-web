namespace AirWeb.Domain.Entities.WorkEntries;

public class PermitRevocation : BaseWorkEntry
{
    internal PermitRevocation(int? id) : base(id) => WorkEntryType = WorkEntryType.PermitRevocation;

    public DateOnly ReceivedDate { get; set; }
    public DateOnly PermitRevocationDate { get; set; }
    public DateOnly? PhysicalShutdownDate { get; set; }
    public bool FollowupTaken { get; set; }
}
