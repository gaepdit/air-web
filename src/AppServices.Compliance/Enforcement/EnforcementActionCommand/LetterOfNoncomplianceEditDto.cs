using AirWeb.AppServices.Core.Utilities;
using AirWeb.Domain.Core.Data.DataAttributes;
using FluentValidation;

namespace AirWeb.AppServices.Compliance.Enforcement.EnforcementActionCommand;

public record LetterOfNoncomplianceEditDto : EnforcementActionEditDto
{
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "Resolved")]
    [MaxDate]
    public DateOnly? ResolvedDate { get; init; }
}

public class LetterOfNoncomplianceEditValidator : AbstractValidator<LetterOfNoncomplianceEditDto>
{
    public LetterOfNoncomplianceEditValidator()
    {
        RuleFor(dto => dto)
           .Must(dto => dto.IssueDate <= DateOnly.FromDateTime(DateTime.Today))
           .WithMessage("The issued date cannot be in the future.")
           .Must(dto => dto.ResolvedDate == null || dto.ResolvedDate <= DateOnly.FromDateTime(DateTime.Today))
           .WithMessage("The resolved date cannot be in the future.")
           .Must(dto => dto.ResolvedDate == null || dto.ResolvedDate >= dto.IssueDate)
           .WithMessage("The resolved date cannot be earlier than the issued date.");
    }
}
