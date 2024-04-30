using MyApp.AppServices.EntryActions.Dto;
using MyApp.AppServices.Staff.Dto;
using MyApp.Domain.Entities.WorkEntries;
using System.ComponentModel.DataAnnotations;

namespace MyApp.AppServices.WorkEntries.QueryDto;

public record WorkEntryViewDto
{
    public Guid Id { get; init; }

    public WorkEntryStatus Status { get; init; }

    [Display(Name = "Date Received")]
    public DateTimeOffset ReceivedDate { get; init; }

    [Display(Name = "Received By")]
    public StaffViewDto? ReceivedBy { get; init; }

    [Display(Name = "Entry Type")]
    public string? EntryTypeName { get; init; }

    public string Notes { get; init; } = string.Empty;

    // Properties: Review/Closure

    [Display(Name = "WorkEntry Closed")]
    public bool Closed { get; init; }

    [Display(Name = "Date Closed")]
    public DateTimeOffset? ClosedDate { get; init; }

    [Display(Name = "Closed By")]
    public StaffViewDto? ClosedBy { get; init; }

    [Display(Name = "Closure Comments")]
    public string? ClosedComments { get; init; }

    // Properties: Deletion

    [Display(Name = "Deleted?")]
    public bool IsDeleted { get; init; }

    [Display(Name = "Deleted By")]
    public StaffViewDto? DeletedBy { get; init; }

    [Display(Name = "Date Deleted")]
    public DateTimeOffset? DeletedAt { get; init; }

    [Display(Name = "Comments")]
    public string? DeleteComments { get; init; }

    // === Lists ===

    [UsedImplicitly]
    public List<EntryActionViewDto> EntryActions { get; } = [];
}
