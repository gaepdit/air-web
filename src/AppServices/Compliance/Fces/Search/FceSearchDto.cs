using AirWeb.AppServices.Compliance.Search;
using IaipDataService.Facilities;

namespace AirWeb.AppServices.Compliance.Fces.Search;

public record FceSearchDto : IComplianceSearchDto
{
    public SortBy Sort { get; init; } = SortBy.IdAsc;

    [Display(Name = "Deletion Status")]
    public DeleteStatus? DeleteStatus { get; set; }

    [Display(Name = "AIRS Number")]
    [StringLength(9)]
    public string? PartialFacilityId { get; init; }

    [Display(Name = "FCE Year")]
    public int? Year { get; init; }

    [Display(Name = "Reviewed By")]
    // Guid as string
    public string? ReviewedBy { get; init; }

    [Display(Name = "Office")]
    public Guid? Office { get; init; }

    [Display(Name = "From")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly? DateFrom { get; init; }

    [Display(Name = "Until")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly? DateTo { get; init; }

    [Display(Name = "On-site inspection conducted")]
    public YesNoAny? Onsite { get; init; }

    [Display(Name = "Notes")]
    public string? Notes { get; init; }

    // UI Routing
    public IDictionary<string, string?> AsRouteValues() => new Dictionary<string, string?>
    {
        { nameof(Sort), Sort.ToString() },
        { nameof(DeleteStatus), DeleteStatus?.ToString() },
        { nameof(PartialFacilityId), PartialFacilityId },
        { nameof(Year), Year.ToString() },
        { nameof(ReviewedBy), ReviewedBy },
        { nameof(Office), Office.ToString() },
        { nameof(DateFrom), DateFrom?.ToString("d") },
        { nameof(DateTo), DateTo?.ToString("d") },
        { nameof(Onsite), Onsite?.ToString() },
        { nameof(Notes), Notes },
    };

    public FceSearchDto TrimAll() => this with
    {
        PartialFacilityId = FacilityId.CleanFacilityId(PartialFacilityId),
        Notes = Notes?.Trim(),
    };
}
