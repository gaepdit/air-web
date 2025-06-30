using AirWeb.AppServices.CommonSearch;
using AirWeb.AppServices.Utilities;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
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

    [Display(Name = "Enforcement Case Status")]
    public CaseFileStatus? CaseFileStatus { get; init; }

    // == Facility ==

    [Display(Name = "AIRS Number")]
    [StringLength(9)]
    public string? PartialFacilityId { get; init; }

    // == Staff ==

    [Display(Name = "Staff")]
    // Guid as string
    public string? Staff { get; init; }

    [Display(Name = "Office")]
    public Guid? Office { get; init; }

    // == Text ==

    [Display(Name = "Notes")]
    public string? Notes { get; init; }

    // == Violation data ==

    public string? ViolationType { get; init; }

    // == Dates ==

    [Display(Name = "From")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    public DateOnly? DateFrom { get; init; }

    [Display(Name = "Until")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    public DateOnly? DateTo { get; init; }

    // UI Routing
    public IDictionary<string, string?> AsRouteValues() => new Dictionary<string, string?>
    {
        { nameof(Sort), Sort.ToString() },
        { nameof(Closed), Closed?.ToString() },
        { nameof(DeleteStatus), DeleteStatus?.ToString() },
        { nameof(CaseFileStatus), CaseFileStatus?.ToString() },
        { nameof(PartialFacilityId), PartialFacilityId },
        { nameof(Staff), Staff },
        { nameof(Office), Office.ToString() },
        { nameof(Notes), Notes },
        { nameof(ViolationType), ViolationType },
        { nameof(DateFrom), DateFrom?.ToString("d") },
        { nameof(DateTo), DateTo?.ToString("d") },
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
