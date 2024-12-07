using AirWeb.Domain.ValueObjects;

namespace AirWeb.AppServices.Reports.ComplianceDto.WorkItems;

public record ReportPartial
{
    [Display(Name = "Tracking #")]
    public int Id { get; init; }

    [Display(Name = "Report period")]
    public string ReportPeriod { get; init; } = "";

    public DateRange ReportPeriodDateRange { get; set; }

    [Display(Name = "Date received")]
    public DateTime ReceivedDate { get; init; }

    public PersonName Reviewer { get; set; }

    [Display(Name = "Deviations reported")]
    public bool DeviationsReported { get; init; }

    public string Comments { get; init; } = "";
}
