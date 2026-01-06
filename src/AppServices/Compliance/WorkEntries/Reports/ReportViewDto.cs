using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Query;
using AirWeb.Domain.ComplianceEntities.ComplianceMonitoring;
using AirWeb.Domain.ValueObjects;

namespace AirWeb.AppServices.Compliance.WorkEntries.Reports;

public record ReportViewDto : ComplianceEventViewDto
{
    [Display(Name = "Date Received")]
    public DateOnly ReceivedDate { get; init; }

    [Display(Name = "Report Type")]
    public ReportingPeriodType ReportingPeriodType { get; init; }

    [Display(Name = "Start")]
    public DateOnly ReportingPeriodStart { get; init; }

    [Display(Name = "End")]
    public DateOnly? ReportingPeriodEnd { get; init; }

    [Display(Name = "Date")]
    public DateRange ReportPeriodDateRange => new(ReportingPeriodStart, ReportingPeriodEnd);

    [Display(Name = "Reporting Period Comment")]
    public string? ReportingPeriodComment { get; init; }

    [Display(Name = "Report Due Date")]
    public DateOnly? DueDate { get; init; }

    [Display(Name = "Date Sent By Facility")]
    public DateOnly? SentDate { get; init; }

    [Display(Name = "Report Is Complete")]
    public bool ReportComplete { get; init; }

    [Display(Name = "Deviations Reported")]
    public bool ReportsDeviations { get; init; }

    [Display(Name = "Enforcement Needed")]
    public bool EnforcementNeeded { get; init; }
}
