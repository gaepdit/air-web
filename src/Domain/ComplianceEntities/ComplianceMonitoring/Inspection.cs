using AirWeb.Domain.DataExchange;
using AirWeb.Domain.Identity;

namespace AirWeb.Domain.ComplianceEntities.ComplianceMonitoring;

public class Inspection : BaseInspection, IDataExchangeAction
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private Inspection() { }

    internal Inspection(int? id, FacilityId facilityId, ApplicationUser? user = null)
        : base(id, facilityId, user)
    {
        ComplianceWorkType = ComplianceWorkType.Inspection;
    }
}
