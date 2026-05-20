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
        RuleFor(dto => dto.IssueDate)
            .Must(date => date <= DateOnly.FromDateTime(DateTime.Today))
            .When(dto => dto.IssueDate.HasValue)
            .WithMessage("The issued date cannot be in the future.");

        RuleFor(dto => dto.IssueDate)
            .NotNull()
            .When(dto => dto.ResolvedDate.HasValue)
            .WithMessage("The resolved date cannot be entered if no issued date is entered.");

        RuleFor(dto => dto.ResolvedDate)
           .Must(date => date <= DateOnly.FromDateTime(DateTime.Today))
           .When(dto => dto.ResolvedDate.HasValue)
           .WithMessage("The resolved date cannot be in the future.");

        RuleFor(dto => dto)
           .Must(dto => dto.ResolvedDate >= dto.IssueDate)
           .When(dto => dto.ResolvedDate.HasValue && dto.IssueDate.HasValue)
           .WithMessage("The resolved date cannot be earlier than the issued date.");
    }
}
