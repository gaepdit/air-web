using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using FluentValidation;

namespace AirWeb.AppServices.Compliance.WorkEntries.Reports;

public class ReportCreateValidator : AbstractValidator<ReportCreateDto>
{
    public ReportCreateValidator()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        RuleFor(dto => dto).SetValidator(new WorkEntryCreateValidator());

        RuleFor(dto => dto.ReceivedDate)
            .Must(date => date <= today)
            .WithMessage("The Received Date cannot be in the future.")
            .Must(date => date.Year >= WorkEntry.EarliestWorkEntryYear)
            .WithMessage($"The Received Date cannot be earlier than {WorkEntry.EarliestWorkEntryYear}.")
            .Must((dto, date) => dto.SentDate is null || date >= dto.SentDate)
            .WithMessage("The Received Date must be later than the Sent Date.");

        RuleFor(dto => dto.ReportingPeriodStart)
            .Must(date => date.Year >= WorkEntry.EarliestWorkEntryYear)
            .WithMessage($"The Reporting Period Start Date cannot be earlier than {WorkEntry.EarliestWorkEntryYear}.");

        RuleFor(dto => dto.ReportingPeriodEnd)
            .Must(date => date <= today)
            .WithMessage("The Reporting Period End Date cannot be in the future.")
            .Must(date => date.Year >= WorkEntry.EarliestWorkEntryYear)
            .WithMessage($"The Reporting Period End Date cannot be earlier than {WorkEntry.EarliestWorkEntryYear}.")
            .Must((dto, date) => date >= dto.ReportingPeriodStart)
            .WithMessage("The Reporting Period End Date must be later than the Start Date.");

        RuleFor(dto => dto.DueDate)
            .Must(date => date is null || date <= today.AddYears(1))
            .WithMessage("The Due Date cannot be more than a year in the future.")
            .Must(date => date is null || date.Value.Year >= WorkEntry.EarliestWorkEntryYear)
            .WithMessage($"The Due Date cannot be earlier than {WorkEntry.EarliestWorkEntryYear}.");

        RuleFor(dto => dto.SentDate)
            .Must(date => date is null || date <= today)
            .WithMessage("The Sent Date cannot be in the future.")
            .Must(date => date is null || date.Value.Year >= WorkEntry.EarliestWorkEntryYear)
            .WithMessage($"The Sent Date cannot be earlier than {WorkEntry.EarliestWorkEntryYear}.")
            .Must((dto, date) => date is null || date >= dto.ReportingPeriodEnd)
            .WithMessage("The Sent Date must be later than the Reporting Period End Date.");
    }
}