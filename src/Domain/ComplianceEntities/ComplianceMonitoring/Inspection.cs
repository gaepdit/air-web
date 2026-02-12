using AirWeb.Core.Entities;
using AirWeb.Domain.DataExchange;

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
