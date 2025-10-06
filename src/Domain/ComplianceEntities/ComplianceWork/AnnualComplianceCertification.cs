using AirWeb.Domain.Identity;

namespace AirWeb.Domain.ComplianceEntities.ComplianceWork;

public class AnnualComplianceCertification : ComplianceEvent
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private AnnualComplianceCertification() { }

    internal AnnualComplianceCertification(int? id, FacilityId facilityId, ApplicationUser? user = null)
        : base(id, facilityId, user)
    {
        WorkEntryType = WorkEntryType.AnnualComplianceCertification;
    }

    // Properties
    private DateOnly _receivedDate;

    public DateOnly ReceivedDate
    {
        get => _receivedDate;
        set
        {
            _receivedDate = value;
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
