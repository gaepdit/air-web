using AirWeb.Domain.Comments;

namespace AirWeb.Domain.ComplianceEntities.WorkEntries;

public record WorkEntryComment : Comment
{
    [UsedImplicitly] // Used by ORM.
    private WorkEntryComment() { }

    private WorkEntryComment(Comment c) : base(c) { }
    public WorkEntryComment(Comment c, int workEntryId) : base(c) => WorkEntryId = workEntryId;
    public int WorkEntryId { get; init; }
}
