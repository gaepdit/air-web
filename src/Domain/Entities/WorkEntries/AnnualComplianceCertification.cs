namespace AirWeb.Domain.Entities.WorkEntries;

public class AnnualComplianceCertification : ComplianceEvent
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private AnnualComplianceCertification() { }

    internal AnnualComplianceCertification(int? id) : base(id)
    {
        WorkEntryType = WorkEntryType.ComplianceEvent;
        ComplianceEventType = ComplianceEventType.AnnualComplianceCertification;
    }

    // Properties
    public DateOnly ReceivedDate { get; set; }
    public int AccReportingYear { get; set; }
    public DateOnly Postmarked { get; set; }
    public bool PostmarkedOnTime { get; set; }
    public bool SignedByRo { get; set; }
    public bool OnCorrectForms { get; set; }
    public bool IncludesAllTvConditions { get; set; }
    public bool CorrectlyCompleted { get; set; }
    public bool ReportsDeviations { get; set; }
    public bool IncludesPreviouslyUnreportedDeviations { get; set; }
    public bool ReportsAllKnownDeviations { get; set; }
    public bool ResubmittalRequired { get; set; }
    public bool EnforcementNeeded { get; set; }
}
