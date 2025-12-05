using AirWeb.Domain.Identity;

namespace AirWeb.Domain.ComplianceEntities.ComplianceWork;

public class Inspection : BaseInspection
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private Inspection() { }

    internal Inspection(int? id, FacilityId facilityId, ApplicationUser? user = null)
        : base(id, facilityId, user)
    {
        WorkEntryType = WorkEntryType.Inspection;
    }
}
