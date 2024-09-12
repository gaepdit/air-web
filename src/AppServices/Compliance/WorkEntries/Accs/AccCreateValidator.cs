using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using FluentValidation;

namespace AirWeb.AppServices.Compliance.WorkEntries.Accs;

public class AccCreateValidator : AbstractValidator<AccCreateDto>
{
    public AccCreateValidator()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        RuleFor(dto => dto).SetValidator(new WorkEntryCreateValidator());

        RuleFor(dto => dto.AccReportingYear)
            .InclusiveBetween(WorkEntry.EarliestWorkEntryYear, today.Year)
            .WithMessage($"The ACC Reporting Year must be between {WorkEntry.EarliestWorkEntryYear} and {today.Year}.");

        RuleFor(dto => dto.ReceivedDate)
            .Must(date => date <= today)
            .WithMessage("The Received Date cannot be in the future.")
            .Must(date => date.Year >= WorkEntry.EarliestWorkEntryYear)
            .WithMessage($"The Received Date cannot be earlier than {WorkEntry.EarliestWorkEntryYear}.")
            .Must((dto, date) => date >= dto.Postmarked)
            .WithMessage("The Received Date must be later than the Postmark Date.");

        RuleFor(dto => dto.Postmarked)
            .Must(date => date <= today)
            .WithMessage("The Postmark Date cannot be in the future.")
            .Must(date => date.Year >= WorkEntry.EarliestWorkEntryYear)
            .WithMessage($"The Postmark Date cannot be earlier than {WorkEntry.EarliestWorkEntryYear}.");

        RuleFor(dto => dto.AcknowledgmentLetterDate)
            .Must((dto, date) => date is null || date >= dto.ReceivedDate)
            .WithMessage("The Acknowledgment Letter Date cannot be earlier than the Received Date.");
    }
}
