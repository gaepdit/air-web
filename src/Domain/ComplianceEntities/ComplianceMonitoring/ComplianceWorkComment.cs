using AirWeb.Domain.Comments;

namespace AirWeb.Domain.ComplianceEntities.ComplianceMonitoring;

public record ComplianceWorkComment : Comment
{
    [UsedImplicitly] // Used by ORM.
    private ComplianceWorkComment() { }

    private ComplianceWorkComment(Comment c) : base(c) { }
    public ComplianceWorkComment(Comment c, int complianceWorkId) : base(c) => ComplianceWorkId = complianceWorkId;
    public int ComplianceWorkId { get; init; }
}
