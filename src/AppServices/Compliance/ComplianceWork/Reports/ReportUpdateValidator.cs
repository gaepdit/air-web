using AirWeb.AppServices.Compliance.ComplianceWork.WorkEntryDto.Command;
using FluentValidation;

namespace AirWeb.AppServices.Compliance.ComplianceWork.Reports;

public class ReportUpdateValidator : AbstractValidator<ReportUpdateDto>
{
    public ReportUpdateValidator(
        IValidator<IWorkEntryCommandDto> workEntryCommandValidator,
        IValidator<ReportCommandDto> reportCommandValidator)
    {
        RuleFor(dto => dto).SetValidator(workEntryCommandValidator);
        RuleFor(dto => dto).SetValidator(reportCommandValidator);
    }
}
