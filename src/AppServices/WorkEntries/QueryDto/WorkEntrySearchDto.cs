using MyApp.Domain.Entities.EntryTypes;
using MyApp.Domain.Entities.WorkEntries;
using System.ComponentModel.DataAnnotations;

namespace MyApp.AppServices.WorkEntries.QueryDto;

public record WorkEntrySearchDto : IBasicSearchDisplay
{
    public SortBy Sort { get; init; } = SortBy.IdAsc;

    [Display(Name = "WorkEntry Status")]
    public WorkEntryStatus? Status { get; init; }

    [Display(Name = "Deletion Status")]
    public SearchDeleteStatus? DeletedStatus { get; set; }

    [Display(Name = "From")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly? ReceivedFrom { get; init; }

    [Display(Name = "Through")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly? ReceivedTo { get; init; }

    [Display(Name = "Received By")]
    public string? ReceivedBy { get; init; }

    [Display(Name = "Entry Type")]
    public Guid? EntryType { get; init; }

    [Display(Name = "Notes text")]
    public string? Text { get; init; }

    // UI Routing
    public IDictionary<string, string?> AsRouteValues() => new Dictionary<string, string?>
    {
        { nameof(Sort), Sort.ToString() },
        { nameof(Status), Status?.ToString() },
        { nameof(DeletedStatus), DeletedStatus?.ToString() },
        { nameof(ReceivedFrom), ReceivedFrom?.ToString("d") },
        { nameof(ReceivedTo), ReceivedTo?.ToString("d") },
        { nameof(EntryType), EntryType?.ToString() },
        { nameof(Text), Text },
    };

    public WorkEntrySearchDto TrimAll() => this with
    {
        Text = Text?.Trim(),
    };
}
