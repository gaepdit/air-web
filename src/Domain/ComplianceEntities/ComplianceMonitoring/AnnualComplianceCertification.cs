using AirWeb.Core.Entities;
using AirWeb.Domain.DataExchange;

namespace AirWeb.Domain.ComplianceEntities.ComplianceMonitoring;

public class AnnualComplianceCertification : ComplianceEvent, IDataExchangeAction
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private AnnualComplianceCertification() { }

    internal AnnualComplianceCertification(int? id, FacilityId facilityId, ApplicationUser? user = null)
        : base(id, facilityId, user)
    {
        ComplianceWorkType = ComplianceWorkType.AnnualComplianceCertification;
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

    // The following are required for new data but nullable for historical data.
    public int? AccReportingYear { get; set; }
    public DateOnly? PostmarkDate { get; set; }
    public bool? PostmarkedOnTime { get; set; }
    public bool? SignedByRo { get; set; }
    public bool? OnCorrectForms { get; set; }
    public bool? IncludesAllTvConditions { get; set; }
    public bool? CorrectlyCompleted { get; set; }
    public bool? ReportsDeviations { get; set; }
    public bool? IncludesPreviouslyUnreportedDeviations { get; set; }
    public bool? ReportsAllKnownDeviations { get; set; }
    public bool? ResubmittalRequired { get; set; }
    public bool? EnforcementNeeded { get; set; }
}
