using AirWeb.Domain.Identity;

namespace AirWeb.Domain.ComplianceEntities.ComplianceWork;

public class PermitRevocation : ComplianceWork
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

    // Required for new data but nullable for historical data.
    public DateOnly? PermitRevocationDate { get; set; }

    public DateOnly? PhysicalShutdownDate { get; set; }
    public bool FollowupTaken { get; set; }
}
