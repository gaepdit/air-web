using AirWeb.AppServices.AuditPoints;
using AirWeb.AppServices.Comments;

namespace AirWeb.AppServices.Compliance.ComplianceMonitoring.WorkEntryDto.Query;

public abstract record WorkEntryViewDto : WorkEntrySummaryDto, IWorkEntryViewDto
{
    public int Id { get; init; }

    [Display(Name = "Date Acknowledgment Letter Sent")]
    public DateOnly? AcknowledgmentLetterDate { get; init; }

    public string Notes { get; init; } = null!;
    public List<CommentViewDto> Comments { get; } = [];
    public List<AuditPointViewDto> AuditPoints { get; } = [];

    public virtual bool HasPrintout => false;
    public virtual string PrintoutPath => string.Empty;
}
