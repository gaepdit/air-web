using FluentValidation;

namespace AirWeb.AppServices.Compliance.ComplianceMonitoring.WorkEntryDto.Command;

// Used by both work entry create and update validators.
public class WorkEntryCommandValidator : AbstractValidator<IWorkEntryCommandDto>
{
    public WorkEntryCommandValidator()
    {
        RuleFor(dto => dto.ResponsibleStaffId).NotEmpty();
        RuleFor(dto => dto.AcknowledgmentLetterDate)
            .Must(date => date is null || date <= DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("The Acknowledgment Letter Date cannot be in the future.");
    }
}
