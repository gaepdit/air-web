using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.ComplianceWorkDto.Command;
using FluentValidation;

namespace AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Reports;

public class ReportUpdateValidator : AbstractValidator<ReportUpdateDto>
{
    public ReportUpdateValidator(
        IValidator<IComplianceWorkCommandDto> complianceWorkCommandValidator,
        IValidator<ReportCommandDto> reportCommandValidator)
    {
        RuleFor(dto => dto).SetValidator(complianceWorkCommandValidator);
        RuleFor(dto => dto).SetValidator(reportCommandValidator);
    }
}
