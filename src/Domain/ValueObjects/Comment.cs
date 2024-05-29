using GaEpd.AppLibrary.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace AirWeb.Domain.ValueObjects;

[Owned]
public record Comment : ValueObject
{
    public Guid Id { get; init; }

    [StringLength(7000)]
    public string Text { get; [UsedImplicitly] init; } = string.Empty;

    public DateTimeOffset CommentedAt { get; [UsedImplicitly] init; } = DateTimeOffset.Now;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Text;
        yield return CommentedAt;
    }
}
