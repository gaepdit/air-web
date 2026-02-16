using AirWeb.Domain.Compliance.DataExchange;
using AirWeb.Domain.Core.Entities;

namespace AirWeb.Domain.Compliance.ComplianceEntities.ComplianceMonitoring;

public class SourceTestReview : ComplianceEvent, IDataExchangeAction
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private SourceTestReview() { }

    internal SourceTestReview(int? id, FacilityId facilityId, ApplicationUser? user = null)
        : base(id, facilityId, user)
    {
        ComplianceWorkType = ComplianceWorkType.SourceTestReview;
        Close(user);
    }

    // Properties

    public int? ReferenceNumber { get; set; }

    public DateOnly ReceivedByComplianceDate
    {
        get;
        set
        {
            field = value;
            EventDate = value;
        }
    }

    public DateOnly? DueDate { get; set; }
    public bool FollowupTaken { get; set; }
}
