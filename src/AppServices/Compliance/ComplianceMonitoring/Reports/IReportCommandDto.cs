using AirWeb.Domain.ComplianceEntities.ComplianceMonitoring;

namespace AirWeb.AppServices.Compliance.ComplianceMonitoring.Reports;

public interface IReportCommandDto
{
    public DateOnly ReceivedDate { get; }
    public ReportingPeriodType ReportingPeriodType { get; }
    public DateOnly ReportingPeriodStart { get; }
    public DateOnly ReportingPeriodEnd { get; }
    public string? ReportingPeriodComment { get; }
    public DateOnly? DueDate { get; }
    public DateOnly? SentDate { get; }
    public bool ReportComplete { get; }
    public bool ReportsDeviations { get; }
    public bool EnforcementNeeded { get; }
}
