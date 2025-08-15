using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Command;
using FluentValidation;

namespace AirWeb.AppServices.Compliance.WorkEntries.Reports;

public class ReportCreateValidator : AbstractValidator<ReportCreateDto>
{
    public ReportCreateValidator(
        IValidator<IWorkEntryCreateDto> workEntryCreateValidator,
        IValidator<ReportCommandDto> reportCommandValidator)
    {
        RuleFor(dto => dto).SetValidator(workEntryCreateValidator);
        RuleFor(dto => dto).SetValidator(reportCommandValidator);
    }
}
