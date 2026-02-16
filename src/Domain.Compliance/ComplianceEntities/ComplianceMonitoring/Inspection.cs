using AirWeb.Domain.Compliance.DataExchange;
using AirWeb.Domain.Core.Entities;

namespace AirWeb.Domain.Compliance.ComplianceEntities.ComplianceMonitoring;

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
