using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.Sbeap.Customers.Dto;

public record CustomerCreateDto
{
    [Required]
    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    [MaxLength(4)]
    [Display(Name = "SIC Code")]
    public string? SicCodeId { get; init; }

    public string? County { get; init; }

    [MaxLength(2000)] // https://stackoverflow.com/q/417142/212978
    public string? Website { get; init; }

    public OptionalAddress Location { get; init; } = null!;
    public OptionalAddress MailingAddress { get; init; } = null!;
    public ContactCreateDto Contact { get; init; } = null!;
}
