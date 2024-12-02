using AirWeb.Domain.Identity;

namespace AirWeb.Domain.ComplianceEntities.WorkEntries;

public class RmpInspection : BaseInspection
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private RmpInspection() { }

    internal RmpInspection(int? id, ApplicationUser? user, FacilityId facilityId) : base(id, user, facilityId)
    {
        WorkEntryType = WorkEntryType.RmpInspection;
    }
}
