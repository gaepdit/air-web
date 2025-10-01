using AirWeb.Domain.Identity;

namespace AirWeb.Domain.Comments;

public record Comment : ISoftDelete<string>
{
    public static Comment CreateComment(string text, ApplicationUser? user) => new()
    {
        Id = Guid.NewGuid(),
        Text = text,
        CommentBy = user,
    };

    // Properties
    public Guid Id { get; init; }

    [StringLength(15_000)]
    public string Text { get; init; } = null!;

    public ApplicationUser? CommentBy { get; init; }
    public DateTimeOffset CommentedAt { get; init; } = DateTimeOffset.Now;

    // Soft delete properties
    public bool IsDeleted => DeletedAt.HasValue;
    public DateTimeOffset? DeletedAt { get; private set; }
    public string? DeletedById { get; private set; }

    public void SetDeleted(string? userId)
    {
        DeletedAt = DateTimeOffset.Now;
        DeletedById = userId;
    }

    public void SetNotDeleted()
    {
        DeletedAt = null;
        DeletedById = null;
    }
}
