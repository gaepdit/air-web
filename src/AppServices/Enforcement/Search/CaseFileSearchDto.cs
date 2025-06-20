using AirWeb.AppServices.CommonSearch;
using AirWeb.AppServices.Utilities;
using GaEpd.AppLibrary.Extensions;
using IaipDataService.Facilities;
using System.ComponentModel;

namespace AirWeb.AppServices.Enforcement.Search;

public record CaseFileSearchDto : ISearchDto<CaseFileSearchDto>, ISearchDto, IDeleteStatus, IClosedStatus
{
    public SortByEnforcement Sort { get; init; } = SortByEnforcement.DiscoveryDateAsc;
    public string SortByName => Sort.ToString();
    public string Sorting => Sort.GetDescription();

    // == Statuses ==

    [Display(Name = "Open/Closed")]
    public ClosedOpenAny? Closed { get; init; }

    [Display(Name = "Deletion Status")]
    public DeleteStatus? DeleteStatus { get; set; }

    // == Facility ==

    [Display(Name = "AIRS Number")]
    [StringLength(9)]
    public string? PartialFacilityId { get; init; }

    // == Staff ==

    [Display(Name = "Staff Responsible")]
    // Guid as string
    public string? ResponsibleStaff { get; init; }

    [Display(Name = "Office")]
    public Guid? Office { get; init; }

    // == Dates ==

    [Display(Name = "From")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    public DateOnly? DiscoveryDateFrom { get; init; }

    [Display(Name = "Until")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    public DateOnly? DiscoveryDateTo { get; init; }

    // == Text ==

    [Display(Name = "Notes")]
    public string? Notes { get; init; }

    // UI Routing
    public IDictionary<string, string?> AsRouteValues() => new Dictionary<string, string?>
    {
        { nameof(Sort), Sort.ToString() },
        { nameof(Closed), Closed?.ToString() },
        { nameof(DeleteStatus), DeleteStatus?.ToString() },
        { nameof(PartialFacilityId), PartialFacilityId },
        { nameof(ResponsibleStaff), ResponsibleStaff },
        { nameof(Office), Office.ToString() },
        { nameof(DiscoveryDateFrom), DiscoveryDateFrom?.ToString("d") },
        { nameof(DiscoveryDateTo), DiscoveryDateTo?.ToString("d") },
        { nameof(Notes), Notes },
    };

    public CaseFileSearchDto TrimAll() => this with
    {
        PartialFacilityId = FacilityId.CleanFacilityId(PartialFacilityId),
        Notes = Notes?.Trim(),
    };
}

public enum SortByEnforcement
{
    [Description("DiscoveryDate, Id")] DiscoveryDateAsc,
    [Description("DiscoveryDate desc, Id")] DiscoveryDateDesc,
    [Description("FacilityId, Id")] FacilityIdAsc,
    [Description("FacilityId desc, Id")] FacilityIdDesc,
}
