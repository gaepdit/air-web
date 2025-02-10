using AirWeb.Domain.Comments;

namespace AirWeb.Domain.EnforcementEntities.CaseFiles;

public record CaseFileComment : Comment
{
    [UsedImplicitly] // Used by ORM.
    private CaseFileComment() { }

    private CaseFileComment(Comment c) : base(c) { }
    public CaseFileComment(Comment c, int caseFileId) : this(c) => CaseFileId = caseFileId;
    public int CaseFileId { get; init; }
}
