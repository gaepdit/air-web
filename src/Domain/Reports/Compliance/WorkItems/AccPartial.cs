using AirWeb.Domain.ValueObjects;

namespace AirWeb.Domain.Reports.Compliance.WorkItems;

public record AccPartial
{
    [Display(Name = "Tracking #")]
    public int Id { get; init; }

    [Display(Name = "Reporting year")]
    public int? AccReportingYear { get; init; }

    public PersonName Reviewer { get; set; }

    [Display(Name = "Date received")]
    public DateTime ReceivedDate { get; init; }

    [Display(Name = "Deviations reported")]
    public bool DeviationsReported { get; init; }
}
