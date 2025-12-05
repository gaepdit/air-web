using AirWeb.Domain;
using FluentValidation;

namespace AirWeb.AppServices.Compliance.ComplianceWork.Inspections;

public class InspectionCommandValidator : AbstractValidator<InspectionCommandDto>
{
    public InspectionCommandValidator()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        RuleFor(dto => dto.InspectionStartedDate)
            .Must(date => date <= today)
            .WithMessage("The Inspection Started Date cannot be in the future.")
            .Must(date => date.Year >= ComplianceConstants.EarliestWorkEntryYear)
            .WithMessage(
                $"The Inspection Started Date cannot be earlier than {ComplianceConstants.EarliestWorkEntryYear}.");

        RuleFor(dto => dto.InspectionEndedDate)
            .Must(date => date <= today)
            .WithMessage("The Inspection End Date cannot be in the future.")
            .Must(date => date.Year >= ComplianceConstants.EarliestWorkEntryYear)
            .WithMessage($"The Inspection End Date cannot be earlier than {ComplianceConstants.EarliestWorkEntryYear}.")
            .Must((dto, date) => date.ToDateTime(dto.InspectionEndedTime) >=
                                 dto.InspectionStartedDate.ToDateTime(dto.InspectionStartedTime))
            .WithMessage("The Inspection End Date must be later than the Start Date.");

        RuleFor(dto => dto.AcknowledgmentLetterDate)
            .Must((dto, date) => date is null || date >= dto.InspectionEndedDate)
            .WithMessage("The Acknowledgment Letter Date cannot be earlier than the Inspection End Date.");
    }
}
