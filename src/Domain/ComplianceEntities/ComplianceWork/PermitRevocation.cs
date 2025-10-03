using AirWeb.Domain.Identity;

namespace AirWeb.Domain.ComplianceEntities.ComplianceWork;

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
    private DateOnly _receivedDate;

    public DateOnly ReceivedDate
    {
        get => _receivedDate;
        set
        {
            _receivedDate = value;
            EventDate = value;
        }
    }

    public DateOnly PermitRevocationDate { get; set; }
    public DateOnly? PhysicalShutdownDate { get; set; }
    public bool FollowupTaken { get; set; }
}
