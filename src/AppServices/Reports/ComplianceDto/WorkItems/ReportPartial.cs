using AirWeb.Domain.ValueObjects;

namespace AirWeb.AppServices.Reports.ComplianceDto.WorkItems;

public record ReportPartial
{
    [Display(Name = "Tracking #")]
    public int Id { get; init; }

    [Display(Name = "Report period")]
    public string ReportPeriod { get; init; } = "";

    public DateRange ReportPeriodDateRange { get; init; }

    [Display(Name = "Date received")]
    public DateOnly ReceivedDate { get; init; }

    public PersonName Reviewer { get; init; }

    [Display(Name = "Deviations reported")]
    public bool DeviationsReported { get; init; }

    public string Notes { get; init; } = "";
}
