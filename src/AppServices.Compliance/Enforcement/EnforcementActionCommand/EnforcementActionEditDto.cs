using AirWeb.AppServices.Core.CommonDtos;
using AirWeb.AppServices.Core.Utilities;
using FluentValidation;

namespace AirWeb.AppServices.Compliance.Enforcement.EnforcementActionCommand;

public record EnforcementActionEditDto : CommentDto
{
    [Display(Name = "Response Requested")]
    public bool ResponseRequested { get; set; } = true;

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "Issued")]
    public DateOnly? IssueDate { get; init; }
}

public abstract class EnforcementActionEditValidator : AbstractValidator<EnforcementActionEditDto>
{
    protected EnforcementActionEditValidator()
    {
        RuleFor(dto => dto.IssueDate)
            .Must(date => date <= DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("The issue date cannot be in the future.");
    }
}
