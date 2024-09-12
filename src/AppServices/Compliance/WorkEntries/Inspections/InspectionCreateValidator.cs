using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using FluentValidation;

namespace AirWeb.AppServices.Compliance.WorkEntries.Inspections;

public class InspectionCreateValidator : AbstractValidator<InspectionCreateDto>
{
    public InspectionCreateValidator()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        RuleFor(dto => dto).SetValidator(new WorkEntryCreateValidator());

        RuleFor(dto => dto.InspectionStartedDate)
            .Must(date => date <= today)
            .WithMessage("The Inspection Started Date cannot be in the future.")
            .Must(date => date.Year >= WorkEntry.EarliestWorkEntryYear)
            .WithMessage($"The Inspection Started Date cannot be earlier than {WorkEntry.EarliestWorkEntryYear}.");

        RuleFor(dto => dto.InspectionEndedDate)
            .Must(date => date <= today)
            .WithMessage("The Inspection End Date cannot be in the future.")
            .Must(date => date.Year >= WorkEntry.EarliestWorkEntryYear)
            .WithMessage($"The Inspection End Date cannot be earlier than {WorkEntry.EarliestWorkEntryYear}.")
            .Must((dto, date) => date.ToDateTime(dto.InspectionEndedTime) >=
                                 dto.InspectionStartedDate.ToDateTime(dto.InspectionStartedTime))
            .WithMessage("The Inspection End Date must be later than the Start Date.");

        RuleFor(dto => dto.AcknowledgmentLetterDate)
            .Must((dto, date) => date is null || date >= dto.InspectionEndedDate)
            .WithMessage("The Acknowledgment Letter Date cannot be earlier than the Inspection End Date.");
    }
}
