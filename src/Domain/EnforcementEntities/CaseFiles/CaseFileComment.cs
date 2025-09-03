using AirWeb.Domain.Comments;

namespace AirWeb.Domain.EnforcementEntities.CaseFiles;

public record CaseFileComment : Comment
{
    [UsedImplicitly] // Used by ORM.
    private CaseFileComment() { }

    public CaseFileComment(Comment c, int caseFileId) : base(c) => CaseFileId = caseFileId;
    public int CaseFileId { get; init; }
}
