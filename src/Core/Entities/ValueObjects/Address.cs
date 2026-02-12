using GaEpd.AppLibrary.Domain.ValueObjects;
using GaEpd.AppLibrary.Extensions;
using Microsoft.EntityFrameworkCore;

namespace AirWeb.Core.Entities.ValueObjects;

[Owned]
public record Address : ValueObject
{
    // Properties

    [Display(Name = "Street Address")]
    public string Street { get; [UsedImplicitly] init; } = string.Empty;

    [Display(Name = "Apt / Suite / Other")]
    public string? Street2 { get; [UsedImplicitly] init; }

    [Display(Name = "City")]
    public string City { get; [UsedImplicitly] init; } = string.Empty;

    [Display(Name = "State")]
    public string State { get; [UsedImplicitly] init; } = string.Empty;

    [DataType(DataType.PostalCode)]
    [Display(Name = "Postal Code")]
    public string PostalCode { get; [UsedImplicitly] init; } = string.Empty;

    // Readonly properties
    public string OneLine => new[]
        {
            Street, Street2, City,
            new[] { State, PostalCode }.ConcatWithSeparator(),
        }
        .ConcatWithSeparator(", ");

    // Empty address
    private static Address EmptyAddress => new();
    public bool IsEmpty => this == EmptyAddress;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Street;
        yield return Street2 ?? string.Empty;
        yield return City;
        yield return State;
        yield return PostalCode;
    }
}
