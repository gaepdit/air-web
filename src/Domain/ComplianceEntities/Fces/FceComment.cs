using AirWeb.Domain.Comments;

namespace AirWeb.Domain.ComplianceEntities.Fces;

public record FceComment : Comment
{
    [UsedImplicitly] // Used by ORM.
    private FceComment() { }

    private FceComment(Comment c) : base(c) { }
    public FceComment(Comment c, int fceId) : this(c) => FceId = fceId;
    public int FceId { get; init; }
}
