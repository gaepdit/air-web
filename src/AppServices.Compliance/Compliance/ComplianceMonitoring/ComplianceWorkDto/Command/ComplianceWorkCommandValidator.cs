using FluentValidation;

namespace AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.ComplianceWorkDto.Command;

// Used by both ComplianceWork create and update validators.
public class ComplianceWorkCommandValidator : AbstractValidator<IComplianceWorkCommandDto>
{
    public ComplianceWorkCommandValidator()
    {
        RuleFor(dto => dto.ResponsibleStaffId).NotEmpty();

        RuleFor(dto => dto.AcknowledgmentLetterDate)
            .Must(date => date <= DateOnly.FromDateTime(DateTime.Today))
            .When(dto => dto.AcknowledgmentLetterDate.HasValue)
            .WithMessage("The Acknowledgment Letter Date cannot be in the future.");
    }
}
