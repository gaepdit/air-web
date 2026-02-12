using AirWeb.Core.Entities;

namespace AirWeb.Domain.ComplianceEntities.ComplianceMonitoring;

public class PermitRevocation : ComplianceWork
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private PermitRevocation() { }

    internal PermitRevocation(int? id, FacilityId facilityId, ApplicationUser? user = null)
        : base(id, facilityId, user)
    {
        ComplianceWorkType = ComplianceWorkType.PermitRevocation;
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

    // Required for new data but nullable for historical data.
    public DateOnly? PermitRevocationDate { get; set; }

    public DateOnly? PhysicalShutdownDate { get; set; }
    public bool FollowupTaken { get; set; }
}
