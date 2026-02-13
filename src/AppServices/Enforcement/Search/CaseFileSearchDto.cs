using AirWeb.AppServices.CommonSearch;
using AirWeb.AppServices.Core.Search;
using AirWeb.AppServices.Utilities;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using GaEpd.AppLibrary.Extensions;
using System.ComponentModel;

namespace AirWeb.AppServices.Enforcement.Search;

public record CaseFileSearchDto : ISearchDto<CaseFileSearchDto>, ISearchDto, IDeleteStatus, IClosedStatus
{
    public CaseFileSortBy Sort { get; init; } = CaseFileSortBy.IdAsc;
    public string SortByName => Sort.ToString();
    public string Sorting => Sort.GetDescription();

    // == Statuses ==

    [Display(Name = "Deletion Status")]
    public DeleteStatus? DeleteStatus { get; set; }

    [Display(Name = "Open/Closed")]
    public ClosedOpenAny? Closed { get; init; }

    [Display(Name = "Enforcement Case Status")]
    public CaseFileStatus? CaseFileStatus { get; init; }

    // == Facility ==

    [Display(Name = "Facility AIRS Number")]
    [StringLength(9)]
    [RegularExpression(IaipDataService.Facilities.FacilityId.SimplifiedFormat,
        ErrorMessage = IaipDataService.Facilities.FacilityId.SimplifiedFormatError)]
    public string? FacilityId { get; init; }

    // == Staff ==

    [Display(Name = "Staff Responsible")]
    public string? Staff { get; init; } // Guid as string

    [Display(Name = "Office")]
    public Guid? Office { get; init; }

    // == Text ==

    [Display(Name = "Notes")]
    public string? Notes { get; init; }

    // == Violation data ==

    [Display(Name = "Violation Type")]
    public string? ViolationType { get; init; }

    // == Discovery date ==

    [Display(Name = "From")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    public DateOnly? DiscoveryDateFrom { get; init; }

    [Display(Name = "Until")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    public DateOnly? DiscoveryDateTo { get; init; }

    // == Initial enforcement date ==

    [Display(Name = "From")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    public DateOnly? EnforcementDateFrom { get; init; }

    [Display(Name = "Until")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    public DateOnly? EnforcementDateTo { get; init; }

    // UI Routing
    public IDictionary<string, string?> AsRouteValues() => new Dictionary<string, string?>
    {
        { nameof(Sort), Sort.ToString() },
        { nameof(Closed), Closed?.ToString() },
        { nameof(DeleteStatus), DeleteStatus?.ToString() },
        { nameof(CaseFileStatus), CaseFileStatus?.ToString() },
        { nameof(FacilityId), FacilityId },
        { nameof(Staff), Staff },
        { nameof(Office), Office.ToString() },
        { nameof(Notes), Notes },
        { nameof(ViolationType), ViolationType },
        { nameof(DiscoveryDateFrom), DiscoveryDateFrom?.ToString("d") },
        { nameof(DiscoveryDateTo), DiscoveryDateTo?.ToString("d") },
        { nameof(EnforcementDateFrom), EnforcementDateFrom?.ToString("d") },
        { nameof(EnforcementDateTo), EnforcementDateTo?.ToString("d") },
    };

    public CaseFileSearchDto TrimAll() => this with
    {
        FacilityId = FacilityId?.Trim(),
        Notes = Notes?.Trim(),
    };
}

public enum CaseFileSortBy
{
    [Description("Id")] IdAsc,
    [Description("Id desc")] IdDesc,
    [Description("FacilityId, Id")] FacilityIdAsc,
    [Description("FacilityId desc, Id")] FacilityIdDesc,
    [Description("DiscoveryDate, Id")] DiscoveryDateAsc,
    [Description("DiscoveryDate desc, Id")] DiscoveryDateDesc,
    [Description("CaseFileStatus, Id")] CaseFileStatusAsc,
    [Description("CaseFileStatus desc, Id")] CaseFileStatusDesc,
}
