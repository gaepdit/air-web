using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Command;
using FluentValidation;

namespace AirWeb.AppServices.Compliance.WorkEntries.Reports;

public class ReportCreateValidator : AbstractValidator<ReportCreateDto>
{
    public ReportCreateValidator()
    {
        RuleFor(dto => dto).SetValidator(new WorkEntryCreateValidator());
        RuleFor(dto => dto).SetValidator(new ReportCommandValidator());
    }
}
