using AirWeb.AppServices.Utilities;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.Compliance.Search;

public record WorkEntrySearchDto : IComplianceSearchDto
{
    public SortBy Sort { get; init; } = SortBy.IdAsc;

    // == Statuses ==

    [Display(Name = "Closed")]
    public YesNoAny? Closed { get; init; }

    [Display(Name = "Deletion Status")]
    public DeleteStatus? DeleteStatus { get; set; }

    // == Work types ==

    public List<WorkTypeSearch> Include { get; init; } = [];

    // == Facility ==

    [Display(Name = "Facility AIRS Number")]
    [StringLength(9)]
    public string? PartialFacilityId { get; init; }

    // == Staff ==

    [Display(Name = "Responsible Staff")]
    // Guid as string
    public string? ResponsibleStaff { get; init; }

    [Display(Name = "Office")]
    public Guid? Office { get; init; }

    // == Dates ==

    [Display(Name = "From")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly? EventDateFrom { get; init; }

    [Display(Name = "Until")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly? EventDateTo { get; init; }

    [Display(Name = "From")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly? ClosedDateFrom { get; init; }

    [Display(Name = "Until")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly? ClosedDateTo { get; init; }

    // == Text ==

    [Display(Name = "Notes")]
    public string? Notes { get; init; }

    // UI Routing
    public IDictionary<string, string?> AsRouteValues()
    {
        var asRouteValues = new Dictionary<string, string?>
        {
            { nameof(Sort), Sort.ToString() },
            { nameof(Closed), Closed?.ToString() },
            { nameof(DeleteStatus), DeleteStatus?.ToString() },
            { nameof(PartialFacilityId), PartialFacilityId },
            { nameof(ResponsibleStaff), ResponsibleStaff },
            { nameof(Office), Office.ToString() },
            { nameof(EventDateFrom), EventDateFrom?.ToString("d") },
            { nameof(EventDateTo), EventDateTo?.ToString("d") },
            { nameof(ClosedDateFrom), ClosedDateFrom?.ToString("d") },
            { nameof(ClosedDateTo), ClosedDateTo?.ToString("d") },
            { nameof(Notes), Notes },
        };

        var i = 0;
        foreach (var workType in Include)
        {
            asRouteValues.Add($"{nameof(Include)}[{i++}]", workType.ToString());
        }

        return asRouteValues;
    }

    public WorkEntrySearchDto TrimAll() => this with
    {
        PartialFacilityId = PartialFacilityId?.CleanFacilityId(),
        Notes = Notes?.Trim(),
    };
}
