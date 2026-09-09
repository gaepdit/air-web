using AirWeb.Domain.Compliance;

namespace AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Inspections;

public class InspectionCommandValidator : AbstractValidator<InspectionCommandDto>
{
    public InspectionCommandValidator()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        RuleFor(dto => dto.InspectionStartedDate)
            .Must(date => date <= today)
            .WithMessage("The Inspection Date cannot be in the future.")
            .Must(date => date.Year >= ComplianceConstants.EarliestComplianceWorkYear)
            .WithMessage(
                $"The Inspection Date cannot be earlier than {ComplianceConstants.EarliestComplianceWorkYear}.");

        RuleFor(dto => dto.InspectionEndedDate)
            .Must(date => date <= today)
            .WithMessage("The Inspection End Date cannot be in the future.")
            .Must(date => date.Year >= ComplianceConstants.EarliestComplianceWorkYear)
            .WithMessage(
                $"The Inspection Date cannot be earlier than {ComplianceConstants.EarliestComplianceWorkYear}.")
            .Must((dto, date) => date >= dto.InspectionStartedDate)
            .WithMessage("The Inspection cannot end before it starts.")
            .When(dto => dto.MultiDayInspection);

        RuleFor(dto => dto.InspectionEndedTime)
            .Must((dto, time) => time >= dto.InspectionStartedTime)
            .WithMessage("The Inspection cannot end before it starts.")
            .When(dto => !dto.MultiDayInspection || dto.InspectionStartedDate == dto.InspectionEndedDate);

        RuleFor(dto => dto.AcknowledgmentLetterDate)
            .Must((dto, date) => date >= dto.InspectionEndedDate)
            .WithMessage("The Acknowledgment Letter Date cannot be earlier than the Inspection End Date.")
            .When(dto => dto.MultiDayInspection, ApplyConditionTo.CurrentValidator)
            .Must((dto, date) => date >= dto.InspectionStartedDate)
            .WithMessage("The Acknowledgment Letter Date cannot be earlier than the Inspection Date.")
            .When(dto => !dto.MultiDayInspection, ApplyConditionTo.CurrentValidator)
            .When(dto => dto.AcknowledgmentLetterDate is not null);
    }
}
