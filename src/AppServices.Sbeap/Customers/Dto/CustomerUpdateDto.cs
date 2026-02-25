using AirWeb.Domain.Core.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.Sbeap.Customers.Dto;

public record CustomerUpdateDto
{
    // Authorization handler assist properties
    public bool IsDeleted { get; [UsedImplicitly] init; }

    // Entity update properties

    [Required]
    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    [MaxLength(4)]
    [Display(Name = "SIC Code")]
    public string? SicCodeId { get; init; }

    public string? County { get; init; }

    [MaxLength(2000)] // https://stackoverflow.com/q/417142/212978
    public string? Website { get; init; }

    public Address Location { get; init; } = null!;
    public Address MailingAddress { get; init; } = null!;
}
