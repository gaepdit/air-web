namespace AirWeb.AppServices.Compliance.ComplianceWork.Accs;

public interface IAccCommandDto
{
    public DateOnly ReceivedDate { get; }
    public int AccReportingYear { get; }
    public DateOnly PostmarkDate { get; }
    public bool PostmarkedOnTime { get; }
    public bool SignedByRo { get; }
    public bool OnCorrectForms { get; }
    public bool IncludesAllTvConditions { get; }
    public bool CorrectlyCompleted { get; }
    public bool ReportsDeviations { get; }
    public bool IncludesPreviouslyUnreportedDeviations { get; }
    public bool ReportsAllKnownDeviations { get; }
    public bool ResubmittalRequired { get; }
    public bool EnforcementNeeded { get; }
}
