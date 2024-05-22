using GaEpd.AppLibrary.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace AirWeb.Domain.ValueObjects;

[Owned]
public record Comment : ValueObject
{
    public int Id { get; init; }

    [StringLength(7000)]
    public string Text { get; [UsedImplicitly] init; } = string.Empty;

    protected override IEnumerable<object> GetEqualityComponents() => new List<object> { Text }.AsReadOnly();
}
