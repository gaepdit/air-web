using AirWeb.AppServices.CommonSearch;
using AirWeb.AppServices.Utilities;
using GaEpd.AppLibrary.Extensions;
using IaipDataService.Facilities;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AirWeb.AppServices.Compliance.Fces.Search;

public record FceSearchDto : ISearchDto<FceSearchDto>, ISearchDto, IDeleteStatus
{
    public FceSortBy Sort { get; init; } = FceSortBy.IdAsc;
    public string SortByName => Sort.ToString();
    public string Sorting => Sort.GetDescription();

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
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    public DateOnly? DateFrom { get; init; }

    [Display(Name = "Until")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    public DateOnly? DateTo { get; init; }

    [Display(Name = "On-Site Inspection Conducted")]
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
        PartialFacilityId = FacilityId.CleanPartialFacilityId(PartialFacilityId),
        Notes = Notes?.Trim(),
    };
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum FceSortBy
{
    [Description("Id")] IdAsc,
    [Description("Id desc")] IdDesc,
    [Description("FacilityId, Id")] FacilityIdAsc,
    [Description("FacilityId desc, Id")] FacilityIdDesc,
    [Description("Year, Id")] YearAsc,
    [Description("Year desc, Id desc")] YearDesc,
}
