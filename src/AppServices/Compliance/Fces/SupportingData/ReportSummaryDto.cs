using AirWeb.AppServices.Staff.Dto;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.Domain.ValueObjects;

namespace AirWeb.AppServices.Compliance.Fces.SupportingData;

public record ReportSummaryDto : BaseSummaryDto
{
    [Display(Name = "Report Period")]
    public ReportingPeriodType ReportingPeriodType { get; init; }

    [Display(Name = "Date Received")]
    public DateOnly ReceivedDate { get; init; }

    public DateOnly ReportingPeriodStart { get; init; }
    public DateOnly? ReportingPeriodEnd { get; init; }
    public DateRange ReportPeriodDateRange => new(ReportingPeriodStart, ReportingPeriodEnd);

    [Display(Name = "Deviations Reported")]
    public bool ReportsDeviations { get; init; }

    [Display(Name = "Reviewer")]
    public StaffViewDto? ResponsibleStaff { get; init; }
}
