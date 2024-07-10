using AirWeb.Domain.Identity;
using GaEpd.AppLibrary.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace AirWeb.Domain.ValueObjects;

[Owned]
public record Comment : ValueObject
{
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
}
