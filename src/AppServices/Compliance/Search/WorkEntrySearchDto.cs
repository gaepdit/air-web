using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.Compliance.Search;

public record WorkEntrySearchDto : IBasicSearchDisplay
{
    public SortBy Sort { get; init; } = SortBy.IdAsc;

    // == Statuses ==

    [Display(Name = "Closed")]
    public YesNoAny? Closed { get; init; }

    [Display(Name = "Deletion Status")]
    public DeleteStatus? DeleteStatus { get; set; }

    // == Work types ==

    public List<WorkEntryTypes> Include { get; init; } = [];

    // == Facility ==

    [Display(Name = "AIRS Number")]
    public string? PartialFacilityId { get; init; }

    // TODO: May need to postpone this feature if it requires too much effort.
    // [Display(Name = "Facility Name")]
    // public string? FacilityName { get; init; }

    // == Staff ==

    [Display(Name = "Responsible Staff")]
    // Guid as string
    public string? ResponsibleStaff { get; init; }

    [Display(Name = "Offices")]
    public List<Guid> Offices { get; init; } = [];

    // == Dates ==

    [Display(Name = "Start Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly? EventDateFrom { get; init; }

    [Display(Name = "End Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly? EventDateTo { get; init; }

    [Display(Name = "Start Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly? ClosedDateFrom { get; init; }

    [Display(Name = "End Date")]
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
            { nameof(EventDateFrom), EventDateFrom?.ToString("d") },
            { nameof(EventDateTo), EventDateTo?.ToString("d") },
            { nameof(ClosedDateFrom), ClosedDateFrom?.ToString("d") },
            { nameof(ClosedDateTo), ClosedDateTo?.ToString("d") },
            { nameof(Notes), Notes },
        };

        foreach (var office in Offices)
            asRouteValues.Add(nameof(Offices), office.ToString());

        foreach (var workType in Include)
            asRouteValues.Add(nameof(Include), workType.ToString());

        return asRouteValues;
    }

    public WorkEntrySearchDto TrimAll() => this with
    {
        PartialFacilityId = PartialFacilityId?.Trim(),
        Notes = Notes?.Trim(),
    };
}
