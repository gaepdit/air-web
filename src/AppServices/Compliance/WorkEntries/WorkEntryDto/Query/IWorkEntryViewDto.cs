using AirWeb.AppServices.Comments;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Query;

public interface IWorkEntryViewDto : IWorkEntrySummaryDto
{
    [Display(Name = "Date Acknowledgment Letter Sent")]
    public DateOnly? AcknowledgmentLetterDate { get; }

    public string Notes { get; }
    public List<CommentViewDto> Comments { get; }

    // Display properties
    public bool HasPrintout { get; }
    public string? PrintoutUrl { get; }
}
