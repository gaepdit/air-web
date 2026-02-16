using FluentValidation;

namespace AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.ComplianceWorkDto.Command;

// Used by both ComplianceWork create and update validators.
public class ComplianceWorkCommandValidator : AbstractValidator<IComplianceWorkCommandDto>
{
    public ComplianceWorkCommandValidator()
    {
        RuleFor(dto => dto.ResponsibleStaffId).NotEmpty();
        RuleFor(dto => dto.AcknowledgmentLetterDate)
            .Must(date => date is null || date <= DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("The Acknowledgment Letter Date cannot be in the future.");
    }
}
