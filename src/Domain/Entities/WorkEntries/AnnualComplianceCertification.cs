namespace AirWeb.Domain.Entities.WorkEntries;

public class AnnualComplianceCertification : BaseWorkEntry
{
    internal AnnualComplianceCertification(int? id) : base(id) =>
        WorkEntryType = WorkEntryType.AnnualComplianceCertification;

    public DateOnly ReceivedDate { get; init; }
    public int AccReportingYear { get; init; }
    public DateOnly PostmarkedDate { get; init; }
    public bool PostmarkedOnTime { get; init; }
    public bool SignedByRo { get; init; }
    public bool OnCorrectForms { get; init; }
    public bool IncludesAllTvConditions { get; init; }
    public bool CorrectlyCompleted { get; init; }
    public bool ReportsDeviations { get; init; }
    public bool IncludesPreviouslyUnreportedDeviations { get; init; }
    public bool ReportsAllKnownDeviations { get; init; }
    public bool ResubmittalRequired { get; init; }
    public bool EnforcementNeeded { get; init; }
}
