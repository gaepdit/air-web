using AirWeb.AppServices.Comments;
using AirWeb.AppServices.Core.AuditPoints;

namespace AirWeb.AppServices.Compliance.ComplianceMonitoring.ComplianceWorkDto.Query;

public abstract record ComplianceWorkViewDto : ComplianceWorkSummaryDto, IComplianceWorkViewDto
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
