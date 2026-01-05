using AirWeb.Domain.Identity;

namespace AirWeb.Domain.ComplianceEntities.ComplianceWork;

public class RmpInspection : BaseInspection
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private RmpInspection() { }

    internal RmpInspection(int? id, FacilityId facilityId, ApplicationUser? user = null)
        : base(id, facilityId, user)
    {
        ComplianceWorkType = ComplianceWorkType.RmpInspection;
    }
}
