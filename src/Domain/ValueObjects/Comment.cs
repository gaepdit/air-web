using AirWeb.Domain.Identity;
using GaEpd.AppLibrary.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace AirWeb.Domain.ValueObjects;

[Owned]
public record Comment : ValueObject, ISoftDelete<string>
{
    public static Comment CreateComment(string text, ApplicationUser? user) => new()
    {
        Id = Guid.NewGuid(),
        Text = text,
        CommentBy = user,
    };

    // Properties
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; init; }

    [StringLength(15_000)]
    public string Text { get; init; } = string.Empty;

    public ApplicationUser? CommentBy { get; init; }
    public DateTimeOffset CommentedAt { get; init; } = DateTimeOffset.Now;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return CommentedAt;
        yield return Text;
        yield return CommentBy ?? new ApplicationUser();
    }

    // Soft delete properties
    public bool IsDeleted { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }
    public string? DeletedById { get; private set; }

    public void SetDeleted(string? userId)
    {
        IsDeleted = true;
        DeletedAt = DateTimeOffset.Now;
        DeletedById = userId;
    }

    public void SetNotDeleted()
    {
        IsDeleted = false;
        DeletedAt = default;
        DeletedById = default;
    }
}
