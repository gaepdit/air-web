using AirWeb.Domain;
using FluentValidation;

namespace AirWeb.AppServices.Compliance.WorkEntries.PermitRevocations;

public class PermitRevocationCommandValidator : AbstractValidator<PermitRevocationCommandDto>
{
    public PermitRevocationCommandValidator()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        RuleFor(dto => dto.ReceivedDate)
            .Must(date => date <= today)
            .WithMessage("The Received Date cannot be in the future.")
            .Must(date => date.Year >= ComplianceConstants.EarliestComplianceWorkYear)
            .WithMessage($"The Received Date cannot be earlier than {ComplianceConstants.EarliestComplianceWorkYear}.");

        RuleFor(dto => dto.PermitRevocationDate)
            .Must(date => date <= today.AddYears(1))
            .WithMessage("The Permit Revocation Date cannot be more than a year in the future.")
            .Must(date => date.Year >= ComplianceConstants.EarliestComplianceWorkYear)
            .WithMessage(
                $"The Permit Revocation Date cannot be earlier than {ComplianceConstants.EarliestComplianceWorkYear}.");

        RuleFor(dto => dto.PhysicalShutdownDate)
            .Must(date => date is null || date <= today.AddYears(1))
            .WithMessage("The Physical Shutdown Date cannot be more than a year in the future.")
            .Must(date => date is null || date.Value.Year >= ComplianceConstants.EarliestComplianceWorkYear)
            .WithMessage($"The Physical Shutdown cannot be earlier than {ComplianceConstants.EarliestComplianceWorkYear}.");

        RuleFor(dto => dto.AcknowledgmentLetterDate)
            .Must((dto, date) => date is null || date >= dto.ReceivedDate)
            .WithMessage("The Acknowledgment Letter Date cannot be earlier than the Received Date.");
    }
}
