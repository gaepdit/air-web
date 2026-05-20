using AirWeb.Domain.Core.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.Sbeap.Customers.Dto;

public record OptionalAddress
{
    [Display(Name = "Street Address")]
    public string? Street { get; [UsedImplicitly] init; }

    [Display(Name = "Apt / Suite / Other")]
    public string? Street2 { get; [UsedImplicitly] init; }

    public string? City { get; [UsedImplicitly] init; }

    public string? State { get; [UsedImplicitly] init; }

    [DataType(DataType.PostalCode)]
    [Display(Name = "Postal Code")]
    public string? PostalCode { get; [UsedImplicitly] init; }

    private static OptionalAddress EmptyAddress { get; } = new();
    public bool IsEmpty => this == EmptyAddress;

    public Address AsAddress() => IsEmpty
        ? new Address()
        : new Address
        {
            Street = Street ?? string.Empty,
            Street2 = Street2 ?? string.Empty,
            City = City ?? string.Empty,
            State = State ?? string.Empty,
            PostalCode = PostalCode ?? string.Empty,
        };
}
