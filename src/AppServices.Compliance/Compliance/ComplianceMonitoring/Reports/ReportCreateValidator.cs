using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.ComplianceWorkDto.Command;

namespace AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Reports;

public class ReportCreateValidator : AbstractValidator<ReportCreateDto>
{
    public ReportCreateValidator(
        IValidator<IComplianceWorkCreateDto> complianceWorkCreateValidator,
        IValidator<ReportCommandDto> reportCommandValidator)
    {
        RuleFor(dto => dto).SetValidator(complianceWorkCreateValidator);
        RuleFor(dto => dto).SetValidator(reportCommandValidator);
    }
}
