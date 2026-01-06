using AirWeb.AppServices.Compliance.ComplianceMonitoring.WorkEntryDto.Command;
using FluentValidation;

namespace AirWeb.AppServices.Compliance.ComplianceMonitoring.Reports;

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
