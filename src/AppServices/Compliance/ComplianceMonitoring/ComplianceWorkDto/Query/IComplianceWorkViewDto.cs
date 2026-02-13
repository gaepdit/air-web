using AirWeb.AppServices.Comments;
using AirWeb.AppServices.Core.AuditPoints;

namespace AirWeb.AppServices.Compliance.ComplianceMonitoring.ComplianceWorkDto.Query;

public interface IComplianceWorkViewDto : IComplianceWorkSummaryDto
{
    [Display(Name = "Date Acknowledgment Letter Sent")]
    public DateOnly? AcknowledgmentLetterDate { get; }

    public string Notes { get; }
    public List<CommentViewDto> Comments { get; }
    public List<AuditPointViewDto> AuditPoints { get; }

    // Display properties
    public bool HasPrintout { get; }
    public string PrintoutPath { get; }
}
