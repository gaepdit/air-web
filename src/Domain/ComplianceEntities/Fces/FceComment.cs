using AirWeb.Domain.Comments;

namespace AirWeb.Domain.ComplianceEntities.Fces;

public record FceComment : Comment
{
    [UsedImplicitly] // Used by ORM.
    private FceComment() { }

    public FceComment(Comment c, int fceId) : base(c) => FceId = fceId;
    public int FceId { get; init; }
}
