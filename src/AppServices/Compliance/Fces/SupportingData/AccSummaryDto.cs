using AirWeb.AppServices.Staff.Dto;

namespace AirWeb.AppServices.Compliance.Fces.SupportingData;

public record AccSummaryDto
{
    [Display(Name = "Tracking #")]
    public int Id { get; init; }

    [Display(Name = "Comments:")]
    public string Notes { get; init; } = null!;

    [Display(Name = "Date received")]
    public DateOnly ReceivedDate { get; init; }

    [Display(Name = "Reporting year")]
    public int AccReportingYear { get; init; }

    [Display(Name = "Deviations reported")]
    public bool ReportsDeviations { get; init; }

    [Display(Name = "Reviewer")]
    public StaffViewDto? ResponsibleStaff { get; init; }
}
