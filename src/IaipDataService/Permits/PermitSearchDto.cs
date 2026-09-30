using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace IaipDataService.Permits;

public record PermitSearchDto
{
    private const string DateOnlyInput = "{0:yyyy-MM-dd}";

    /// <summary>A partial facility name to search for.</summary>
    [Display(Name = "Facility Name")]
    [StringLength(100)]
    public string? Name { get; init; }

    /// <summary>A partial permit number (or SIC code) to search for.</summary>
    [Display(Name = "Permit Number or SIC Code")]
    [StringLength(20)]
    public string? Permit { get; init; }

    /// <summary>A starting permit issuance date to search from.</summary>
    [Display(Name = "From")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateOnlyInput, ApplyFormatInEditMode = true)]
    public DateOnly? DateFrom { get; init; }

    /// <summary>An ending permit issuance date to search through.</summary>
    [Display(Name = "Until")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateOnlyInput, ApplyFormatInEditMode = true)]
    public DateOnly? DateTo { get; init; }

    public PermitSearchDto TrimAll() => this with
    {
        Name = Name?.Trim(),
        Permit = Permit?.Trim(),
    };
}

public class PermitSearchValidator : AbstractValidator<PermitSearchDto>
{
    public PermitSearchValidator()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        RuleFor(dto => dto.DateFrom)
            .Must(date => date <= today || date == null)
            .WithMessage("The beginning search date cannot be in the future.");

        RuleFor(dto => dto.DateTo)
            .Must((dto, date) => date >= dto.DateFrom)
            .When(dto => dto.DateFrom.HasValue && dto.DateTo.HasValue)
            .WithMessage("The end search date must be later than the beginning date.");
    }
}
