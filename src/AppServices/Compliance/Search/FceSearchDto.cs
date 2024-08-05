using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.Compliance.Search;

public record FceSearchDto : IBasicSearchDisplay, IDeleteStatusSearch
{
    public SortBy Sort { get; init; } = SortBy.IdAsc;

    [Display(Name = "Deletion Status")]
    public DeleteStatus? DeleteStatus { get; set; }

    [Display(Name = "AIRS Number")]
    public string? PartialFacilityId { get; init; }

    // TODO: May need to postpone this feature if it requires too much effort.
    // [Display(Name = "Facility Name")]
    // public string? FacilityName { get; init; }

    [Display(Name = "FCE Year")]
    public int? Year { get; init; }

    [Display(Name = "Reviewed By")]
    // Guid as string
    public string? ReviewedBy { get; init; }

    [Display(Name = "Offices")]
    public List<Guid> Offices { get; init; } = [];

    [Display(Name = "Start Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly? DateFrom { get; init; }

    [Display(Name = "End Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly? DateTo { get; init; }

    [Display(Name = "On-site inspection conducted")]
    public YesNoAny? Onsite { get; init; }

    [Display(Name = "Notes")]
    public string? Notes { get; init; }

    // UI Routing
    public IDictionary<string, string?> AsRouteValues()
    {
        var asRouteValues = new Dictionary<string, string?>
        {
            { nameof(Sort), Sort.ToString() },
            { nameof(DeleteStatus), DeleteStatus?.ToString() },
            { nameof(PartialFacilityId), PartialFacilityId },
            { nameof(Year), Year.ToString() },
            { nameof(ReviewedBy), ReviewedBy },
            { nameof(DateFrom), DateFrom?.ToString("d") },
            { nameof(DateTo), DateTo?.ToString("d") },
            { nameof(Onsite), Onsite?.ToString() },
            { nameof(Notes), Notes },
        };

        foreach (var office in Offices)
            asRouteValues.Add(nameof(Offices), office.ToString());

        return asRouteValues;
    }

    public FceSearchDto TrimAll() => this with
    {
        PartialFacilityId = PartialFacilityId?.Trim(),
        Notes = Notes?.Trim(),
    };
}
