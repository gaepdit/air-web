using AirWeb.Domain.Compliance;
using FluentValidation;

namespace AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Accs;

public class AccCommandValidator : AbstractValidator<AccCommandDto>
{
    public AccCommandValidator()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        RuleFor(dto => dto.AccReportingYear)
            .InclusiveBetween(ComplianceConstants.EarliestComplianceWorkYear, today.Year)
            .WithMessage(
                $"The ACC Reporting Year must be between {ComplianceConstants.EarliestComplianceWorkYear} and {today.Year}.");

        RuleFor(dto => dto.ReceivedDate)
            .Must(date => date <= today)
            .WithMessage("The received date cannot be in the future.")
            .Must(date => date.Year >= ComplianceConstants.EarliestComplianceWorkYear)
            .WithMessage($"The received date cannot be earlier than {ComplianceConstants.EarliestComplianceWorkYear}.")
            .Must((dto, date) => date >= dto.PostmarkDate)
            .WithMessage("The received date must be later than the postmark date.");

        RuleFor(dto => dto.PostmarkDate)
            .Must(date => date <= today)
            .WithMessage("The postmark date cannot be in the future.")
            .Must(date => date.Year >= ComplianceConstants.EarliestComplianceWorkYear)
            .WithMessage($"The postmark date cannot be earlier than {ComplianceConstants.EarliestComplianceWorkYear}.");

        RuleFor(dto => dto.ReviewedDate)
            .Must(date => date <= today)
            .When(dto => dto.ReviewedDate.HasValue)
            .WithMessage("The date reviewed cannot be in the future.");

        RuleFor(dto => dto.ReviewedDate)
            .Must(date => date is null || date.Value.Year >= ComplianceConstants.EarliestComplianceWorkYear)
            .WithMessage($"The date reviewed cannot be earlier than {ComplianceConstants.EarliestComplianceWorkYear}.");

        RuleFor(dto => dto.ReviewedDate)
            .Must((dto, date) => date >= dto.ReceivedDate)
            .When(dto => dto.ReviewedDate.HasValue)
            .WithMessage("The date reviewed cannot be earlier than the date received.");

        RuleFor(dto => dto.AcknowledgmentLetterDate)
            .Must((dto, date) => date >= dto.ReceivedDate)
            .When(dto => dto.AcknowledgmentLetterDate.HasValue)
            .WithMessage("The acknowledgment letter date cannot be earlier than the received date.");
    }
}
