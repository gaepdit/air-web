using AirWeb.AppServices.Core.CommonDtos;
using AirWeb.AppServices.Core.Utilities;
using AirWeb.Domain.Core.Data.DataAttributes;
using FluentValidation;

namespace AirWeb.AppServices.Compliance.Enforcement.EnforcementActionCommand;

public record EnforcementActionEditDto : NotesDto
{
    [Display(Name = "Response Requested")]
    public bool ResponseRequested { get; set; } = true;

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "Issued")]
    [MaxDate]
    public DateOnly? IssueDate { get; init; }
}

public class EnforcementActionEditValidator : AbstractValidator<EnforcementActionEditDto>
{
    public EnforcementActionEditValidator()
    {
        RuleFor(dto => dto.IssueDate)
            .Must(date => date <= DateOnly.FromDateTime(DateTime.Today))
            .When(dto => dto.IssueDate.HasValue)
            .WithMessage("The issued date cannot be in the future.");
    }
}
