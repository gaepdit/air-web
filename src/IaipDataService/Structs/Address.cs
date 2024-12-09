using IaipDataService.Utilities;
using System.ComponentModel.DataAnnotations;

namespace IaipDataService.Structs;

public readonly record struct Address
{
    // Properties

    [Display(Name = "Street Address")]
    public string Street { get; [UsedImplicitly] init; }

    [Display(Name = "Apt / Suite / Other")]
    public string? Street2 { get; [UsedImplicitly] init; }

    [Display(Name = "City")]
    public string City { get; [UsedImplicitly] init; }

    [Display(Name = "State")]
    public string State { get; [UsedImplicitly] init; }

    [DataType(DataType.PostalCode)]
    [Display(Name = "Postal Code")]
    public string PostalCode { get; [UsedImplicitly] init; }

    // Readonly properties
    public string OneLine => new[]
        {
            Street, Street2, City,
            new[] { State, PostalCode }.ConcatWithSeparator(),
        }
        .ConcatWithSeparator(", ");

    public string CityState => new[] { City, State }.ConcatWithSeparator(", ");
}
