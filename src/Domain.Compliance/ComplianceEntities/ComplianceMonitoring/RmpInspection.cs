using AirWeb.Domain.Core.Entities;

namespace AirWeb.Domain.Compliance.ComplianceEntities.ComplianceMonitoring;

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
