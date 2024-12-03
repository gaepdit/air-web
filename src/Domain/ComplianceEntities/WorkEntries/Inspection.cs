using AirWeb.Domain.Identity;

namespace AirWeb.Domain.ComplianceEntities.WorkEntries;

public class Inspection : BaseInspection
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private Inspection() { }

    internal Inspection(int? id, ApplicationUser? user, FacilityId facilityId) : base(id, user, facilityId)
    {
        WorkEntryType = WorkEntryType.Inspection;
    }
}
