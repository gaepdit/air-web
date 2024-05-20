using AirWeb.AppServices.Staff.Dto;
using AirWeb.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.WorkEntries.ViewDto;

public record WorkEntryViewDto : IWorkEntryViewDto
{
    public int Id { get; init; }

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

    public List<Comment> Comments { get; } = [];
}
