using System.ComponentModel.DataAnnotations;

namespace IaipDataService.ValueObjects;

public record Address
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

    // Empty address
    private static Address EmptyAddress => new();
    public bool IsEmpty => this == EmptyAddress;
}
