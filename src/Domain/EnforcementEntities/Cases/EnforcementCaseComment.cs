using AirWeb.Domain.Comments;

namespace AirWeb.Domain.EnforcementEntities.Cases;

public record EnforcementCaseComment : Comment
{
    [UsedImplicitly] // Used by ORM.
    private EnforcementCaseComment() { }

    private EnforcementCaseComment(Comment c) : base(c) { }
    public EnforcementCaseComment(Comment c, int enforcementCaseId) : this(c) => EnforcementCaseId = enforcementCaseId;
    public int EnforcementCaseId { get; init; }
}
