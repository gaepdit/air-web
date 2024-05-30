namespace AirWeb.AppServices.WorkEntries.Accs;

public interface IAccCommandDto
{
    public bool IsClosed { get; }
    public DateOnly ReceivedDate { get; }
    public int AccReportingYear { get; }
    public DateOnly Postmarked { get; }
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
