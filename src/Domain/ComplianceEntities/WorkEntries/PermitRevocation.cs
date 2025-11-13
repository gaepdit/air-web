using AirWeb.Domain.Identity;

namespace AirWeb.Domain.ComplianceEntities.WorkEntries;

public class PermitRevocation : WorkEntry
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private PermitRevocation() { }

    internal PermitRevocation(int? id, FacilityId facilityId, ApplicationUser? user = null)
        : base(id, facilityId, user)
    {
        WorkEntryType = WorkEntryType.PermitRevocation;
    }

    // Properties

    public DateOnly ReceivedDate
    {
        get;
        set
        {
            field = value;
            EventDate = value;
        }
    }

    public DateOnly PermitRevocationDate { get; set; }
    public DateOnly? PhysicalShutdownDate { get; set; }
    public bool FollowupTaken { get; set; }
}
