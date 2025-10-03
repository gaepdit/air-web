using AirWeb.AppServices.Compliance.ComplianceWork.WorkEntryDto.Command;
using FluentValidation;

namespace AirWeb.AppServices.Compliance.ComplianceWork.Reports;

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
