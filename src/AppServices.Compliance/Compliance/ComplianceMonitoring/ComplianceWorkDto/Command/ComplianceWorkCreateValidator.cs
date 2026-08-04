namespace AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.ComplianceWorkDto.Command;

public class ComplianceWorkCreateValidator : AbstractValidator<IComplianceWorkCreateDto>
{
    public ComplianceWorkCreateValidator(IValidator<IComplianceWorkCommandDto> complianceWorkCommandValidator)
    {
        RuleFor(dto => dto).Must(dto => dto.FacilityId != null || dto.CaseFileId != null);
        RuleFor(dto => dto).SetValidator(complianceWorkCommandValidator);
    }
}
