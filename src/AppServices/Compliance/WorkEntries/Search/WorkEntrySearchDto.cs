using AirWeb.AppServices.CommonSearch;
using AirWeb.AppServices.Utilities;
using GaEpd.AppLibrary.Extensions;
using IaipDataService.Facilities;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AirWeb.AppServices.Compliance.WorkEntries.Search;

public record WorkEntrySearchDto : ISearchDto<WorkEntrySearchDto>, ISearchDto, IDeleteStatus, IClosedStatus
{
    public WorkEntrySortBy Sort { get; init; } = WorkEntrySortBy.IdAsc;
    public string SortByName => Sort.ToString();
    public string Sorting => Sort.GetDescription();

    // == Statuses ==

    [Display(Name = "Open/Closed")]
    public ClosedOpenAny? Closed { get; init; }

    [Display(Name = "Deletion Status")]
    public DeleteStatus? DeleteStatus { get; set; }

    // == Work types ==

    public List<WorkTypeSearch> Include { get; init; } = [];

    // == Facility ==

    [Display(Name = "Facility AIRS Number")]
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
    public DateOnly? EventDateFrom { get; init; }

    [Display(Name = "Until")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    public DateOnly? EventDateTo { get; init; }

    [Display(Name = "From")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    public DateOnly? ClosedDateFrom { get; init; }

    [Display(Name = "Until")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
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
        PartialFacilityId = FacilityId.CleanPartialFacilityId(PartialFacilityId),
        Notes = Notes?.Trim(),
    };
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum WorkTypeSearch
{
    [Display(Name = "Annual Compliance Certifications")] Acc,
    [Display(Name = "Inspections")] Inspection,
    [Display(Name = "RMP Inspections")] Rmp,
    [Display(Name = "Reports")] Report,
    [Display(Name = "Source Test Compliance Reviews")] Str,
    [Display(Name = "Notifications")] Notification,
    [Display(Name = "Permit Revocations")] PermitRevocation,
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum WorkEntrySortBy
{
    [Description("Id")] IdAsc,
    [Description("Id desc")] IdDesc,
    [Description("FacilityId, Id")] FacilityIdAsc,
    [Description("FacilityId desc, Id")] FacilityIdDesc,
    [Description("WorkEntryType, Id")] WorkTypeAsc,
    [Description("WorkEntryType desc, Id")] WorkTypeDesc,
    [Description("EventDate, Id")] EventDateAsc,
    [Description("EventDate desc, Id")] EventDateDesc,
}
